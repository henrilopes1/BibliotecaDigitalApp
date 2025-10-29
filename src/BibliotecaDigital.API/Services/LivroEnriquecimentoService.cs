using BibliotecaDigital.API.DTOs;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;

namespace BibliotecaDigital.API.Services
{
    public interface ILivroEnriquecimentoService
    {
        Task<LivroEnriquecidoDto?> EnriquecerLivroAsync(int livroId);
        Task<List<LivroExternoDto>> BuscarLivrosExternosAsync(FiltrosBuscaExternaDto filtros);
        Task<LivroDTO?> ImportarLivroExternoAsync(ImportarLivroExternoDto importacao);
    }

    public class LivroEnriquecimentoService : ILivroEnriquecimentoService
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly IOpenLibraryService _openLibraryService;
        private readonly ILogger<LivroEnriquecimentoService> _logger;

        public LivroEnriquecimentoService(
            ILivroRepository livroRepository,
            IAutorRepository autorRepository,
            IOpenLibraryService openLibraryService,
            ILogger<LivroEnriquecimentoService> logger)
        {
            _livroRepository = livroRepository;
            _autorRepository = autorRepository;
            _openLibraryService = openLibraryService;
            _logger = logger;
        }

        public async Task<LivroEnriquecidoDto?> EnriquecerLivroAsync(int livroId)
        {
            try
            {
                _logger.LogInformation("Enriquecendo dados do livro ID: {LivroId}", livroId);

                // Buscar livro interno
                var livro = await _livroRepository.GetByIdAsync(livroId);
                if (livro == null)
                {
                    _logger.LogWarning("Livro não encontrado: {LivroId}", livroId);
                    return null;
                }

                // Buscar dados externos na Open Library
                var filtrosBusca = new FiltrosBuscaExternaDto
                {
                    Titulo = livro.Titulo,
                    Autor = livro.Autor?.Nome,
                    Limite = 5
                };

                var resultadosBusca = await _openLibraryService.BuscarLivrosAsync(filtrosBusca);
                var livroExterno = resultadosBusca.Docs.FirstOrDefault();

                // Criar DTO enriquecido
                var livroEnriquecido = new LivroEnriquecidoDto
                {
                    Id = livro.Id,
                    Titulo = livro.Titulo,
                    Categoria = livro.Genero ?? "Geral",
                    DataPublicacao = livro.AnoPublicacao.HasValue ? new DateTime(livro.AnoPublicacao.Value, 1, 1) : DateTime.MinValue,
                    Disponivel = livro.Ativo && livro.EstoqueDisponivel > 0,
                    Autor = new AutorDTO
                    {
                        Id = livro.Autor?.Id ?? 0,
                        Nome = livro.Autor?.Nome ?? "Desconhecido",
                        Email = livro.Autor?.Email,
                        DataNascimento = livro.Autor?.DataNascimento,
                        DataCriacao = DateTime.Now
                    }
                };

                // Enriquecer com dados externos se encontrados
                if (livroExterno != null)
                {
                    livroEnriquecido.DadosExternos = MapearDadosExternos(livroExterno);
                    
                    // Buscar capas
                    if (livroExterno.Cover_I?.Any() == true)
                    {
                        livroEnriquecido.Capas = await _openLibraryService.ObterCapasPorIdOpenLibraryAsync(livroExterno.Cover_I.First());
                    }
                    else if (livroExterno.Isbn?.Any() == true)
                    {
                        livroEnriquecido.Capas = await _openLibraryService.ObterCapasAsync(livroExterno.Isbn.First());
                    }
                }

                _logger.LogInformation("Livro enriquecido com sucesso: {LivroId}", livroId);
                return livroEnriquecido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enriquecer livro: {LivroId}", livroId);
                return null;
            }
        }

        public async Task<List<LivroExternoDto>> BuscarLivrosExternosAsync(FiltrosBuscaExternaDto filtros)
        {
            try
            {
                _logger.LogInformation("Buscando livros externos com filtros: {@Filtros}", filtros);

                var resultados = await _openLibraryService.BuscarLivrosAsync(filtros);
                var livrosExternos = new List<LivroExternoDto>();

                // Buscar todos os livros internos para verificar duplicatas
                var livrosInternos = await _livroRepository.GetAllAsync();

                foreach (var livroExterno in resultados.Docs.Take(filtros.Limite))
                {
                    var livroExternoDto = await MapearLivroExternoAsync(livroExterno, livrosInternos);
                    livrosExternos.Add(livroExternoDto);
                }

                _logger.LogInformation("Mapeados {Count} livros externos", livrosExternos.Count);
                return livrosExternos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar livros externos");
                return new List<LivroExternoDto>();
            }
        }

        public async Task<LivroDTO?> ImportarLivroExternoAsync(ImportarLivroExternoDto importacao)
        {
            try
            {
                _logger.LogInformation("Importando livro externo: {Chave}", importacao.ChaveOpenLibrary);

                // Buscar dados do livro na Open Library
                var livroExterno = await _openLibraryService.ObterLivroPorChaveAsync(importacao.ChaveOpenLibrary);
                if (livroExterno == null)
                {
                    _logger.LogWarning("Livro não encontrado na Open Library: {Chave}", importacao.ChaveOpenLibrary);
                    return null;
                }

                // Verificar se o autor existe
                var autor = await _autorRepository.GetByIdAsync(importacao.AutorId);
                if (autor == null)
                {
                    _logger.LogWarning("Autor não encontrado: {AutorId}", importacao.AutorId);
                    return null;
                }

                // Criar novo livro interno
                var novoLivro = new Livro
                {
                    Titulo = livroExterno.Title,
                    Genero = importacao.CategoriaPersonalizada ?? 
                               (livroExterno.Subject?.FirstOrDefault() ?? "Geral"),
                    AnoPublicacao = livroExterno.First_Publish_Year,
                    AutorId = importacao.AutorId,
                    Ativo = true,
                    EstoqueDisponivel = 1,
                    EstoqueTotal = 1
                };

                var livroSalvo = await _livroRepository.AddAsync(novoLivro);

                _logger.LogInformation("Livro importado com sucesso: {LivroId}", livroSalvo.Id);

                return new LivroDTO
                {
                    Id = livroSalvo.Id,
                    Titulo = livroSalvo.Titulo,
                    Genero = livroSalvo.Genero,
                    AnoPublicacao = livroSalvo.AnoPublicacao,
                    AutorId = livroSalvo.AutorId,
                    Ativo = livroSalvo.Ativo,
                    EstoqueDisponivel = livroSalvo.EstoqueDisponivel,
                    EstoqueTotal = livroSalvo.EstoqueTotal,
                    DataCriacao = livroSalvo.DataCriacao
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao importar livro externo: {Chave}", importacao.ChaveOpenLibrary);
                return null;
            }
        }

        private static DadosEnriquecidosDto MapearDadosExternos(OpenLibraryBookDto livroExterno)
        {
            return new DadosEnriquecidosDto
            {
                AutoresExternos = livroExterno.Author_Name ?? new List<string>(),
                Categorias = livroExterno.Subject?.Take(10).ToList() ?? new List<string>(),
                Editoras = livroExterno.Publisher?.Take(5).ToList() ?? new List<string>(),
                NumeroPaginas = livroExterno.Number_Of_Pages_Median,
                AvaliacaoMedia = livroExterno.Ratings_Average,
                NumeroAvaliacoes = livroExterno.Ratings_Count,
                Idiomas = !string.IsNullOrEmpty(livroExterno.Language) 
                         ? new List<string> { livroExterno.Language } 
                         : new List<string>(),
                TemTextoCompleto = livroExterno.Has_Fulltext,
                ISBNs = livroExterno.Isbn?.Take(5).ToList() ?? new List<string>(),
                ChaveOpenLibrary = livroExterno.Key
            };
        }

        private async Task<LivroExternoDto> MapearLivroExternoAsync(OpenLibraryBookDto livroExterno, IEnumerable<Livro> livrosInternos)
        {
            var livroExternoDto = new LivroExternoDto
            {
                ChaveOpenLibrary = livroExterno.Key,
                Titulo = livroExterno.Title,
                Autores = livroExterno.Author_Name ?? new List<string>(),
                AnoPublicacao = livroExterno.First_Publish_Year,
                Categorias = livroExterno.Subject?.Take(5).ToList() ?? new List<string>(),
                ISBNs = livroExterno.Isbn?.Take(3).ToList() ?? new List<string>(),
                AvaliacaoMedia = livroExterno.Ratings_Average,
                NumeroAvaliacoes = livroExterno.Ratings_Count
            };

            // Verificar se já existe no banco interno
            var livroExistente = livrosInternos.FirstOrDefault(l => 
                l.Titulo.Equals(livroExterno.Title, StringComparison.OrdinalIgnoreCase));

            if (livroExistente != null)
            {
                livroExternoDto.ExisteNoBanco = true;
                livroExternoDto.IdLivroInterno = livroExistente.Id;
            }

            // Buscar capas
            if (livroExterno.Cover_I?.Any() == true)
            {
                livroExternoDto.Capas = await _openLibraryService.ObterCapasPorIdOpenLibraryAsync(livroExterno.Cover_I.First());
            }
            else if (livroExterno.Isbn?.Any() == true)
            {
                livroExternoDto.Capas = await _openLibraryService.ObterCapasAsync(livroExterno.Isbn.First());
            }

            return livroExternoDto;
        }
    }
}
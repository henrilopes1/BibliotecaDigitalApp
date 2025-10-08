using BibliotecaDigital.API.DTOs;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

namespace BibliotecaDigital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IAutorRepository _autorRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public LivrosController(ILivroRepository livroRepository, IAutorRepository autorRepository, IHttpClientFactory httpClientFactory)
        {
            _livroRepository = livroRepository;
            _autorRepository = autorRepository;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Obtém todos os livros
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetAll()
        {
            var livros = await _livroRepository.GetAllAsync();
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Obtém um livro por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LivroDTO>> GetById(int id)
        {
            var livro = await _livroRepository.GetByIdAsync(id);
            if (livro == null)
                return NotFound($"Livro com ID {id} não encontrado.");

            return Ok(MapToDTO(livro));
        }

        /// <summary>
        /// Busca livros por título
        /// </summary>
        [HttpGet("buscar/{titulo}")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> SearchByTitle(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return BadRequest("Título para busca não pode ser vazio.");

            var livros = await _livroRepository.SearchByTitleAsync(titulo);
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Obtém livros por autor
        /// </summary>
        [HttpGet("autor/{autorId}")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetByAutor(int autorId)
        {
            var autorExiste = await _autorRepository.ExistsAsync(autorId);
            if (!autorExiste)
                return NotFound($"Autor com ID {autorId} não encontrado.");

            var livros = await _livroRepository.GetByAutorIdAsync(autorId);
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Obtém livros disponíveis (com estoque > 0)
        /// </summary>
        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetAvailable()
        {
            var livros = await _livroRepository.GetAvailableAsync();
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Cria um novo livro
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LivroDTO>> Create([FromBody] CreateLivroDTO createLivroDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se o autor existe
            var autorExiste = await _autorRepository.ExistsAsync(createLivroDTO.AutorId);
            if (!autorExiste)
                return BadRequest($"Autor com ID {createLivroDTO.AutorId} não encontrado.");

            var livro = new Livro
            {
                Titulo = createLivroDTO.Titulo,
                AutorId = createLivroDTO.AutorId,
                ISBN = createLivroDTO.ISBN,
                AnoPublicacao = createLivroDTO.AnoPublicacao,
                Editora = createLivroDTO.Editora,
                Genero = createLivroDTO.Genero,
                NumeroEdicao = createLivroDTO.NumeroEdicao,
                NumeroPaginas = createLivroDTO.NumeroPaginas,
                Idioma = createLivroDTO.Idioma,
                Sinopse = createLivroDTO.Sinopse,
                CapaUrl = createLivroDTO.CapaUrl,
                Preco = createLivroDTO.Preco,
                EstoqueDisponivel = createLivroDTO.EstoqueDisponivel,
                EstoqueTotal = createLivroDTO.EstoqueTotal,
                Ativo = true
            };

            var novoLivro = await _livroRepository.AddAsync(livro);
            var livroDTO = MapToDTO(novoLivro);

            return CreatedAtAction(nameof(GetById), new { id = novoLivro.Id }, livroDTO);
        }

        /// <summary>
        /// Atualiza um livro existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<LivroDTO>> Update(int id, [FromBody] UpdateLivroDTO updateLivroDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var livroExistente = await _livroRepository.GetByIdAsync(id);
            if (livroExistente == null)
                return NotFound($"Livro com ID {id} não encontrado.");

            // Verificar se o autor existe
            var autorExiste = await _autorRepository.ExistsAsync(updateLivroDTO.AutorId);
            if (!autorExiste)
                return BadRequest($"Autor com ID {updateLivroDTO.AutorId} não encontrado.");

            livroExistente.Titulo = updateLivroDTO.Titulo;
            livroExistente.AutorId = updateLivroDTO.AutorId;
            livroExistente.ISBN = updateLivroDTO.ISBN;
            livroExistente.AnoPublicacao = updateLivroDTO.AnoPublicacao;
            livroExistente.Editora = updateLivroDTO.Editora;
            livroExistente.Genero = updateLivroDTO.Genero;
            livroExistente.NumeroEdicao = updateLivroDTO.NumeroEdicao;
            livroExistente.NumeroPaginas = updateLivroDTO.NumeroPaginas;
            livroExistente.Idioma = updateLivroDTO.Idioma;
            livroExistente.Sinopse = updateLivroDTO.Sinopse;
            livroExistente.CapaUrl = updateLivroDTO.CapaUrl;
            livroExistente.Preco = updateLivroDTO.Preco;
            livroExistente.EstoqueDisponivel = updateLivroDTO.EstoqueDisponivel;
            livroExistente.EstoqueTotal = updateLivroDTO.EstoqueTotal;
            livroExistente.Ativo = updateLivroDTO.Ativo;

            var livroAtualizado = await _livroRepository.UpdateAsync(livroExistente);
            return Ok(MapToDTO(livroAtualizado));
        }

        /// <summary>
        /// Exclui um livro (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var livroExiste = await _livroRepository.ExistsAsync(id);
            if (!livroExiste)
                return NotFound($"Livro com ID {id} não encontrado.");

            var sucesso = await _livroRepository.DeleteAsync(id);
            if (!sucesso)
                return BadRequest("Não foi possível excluir o livro.");

            return NoContent();
        }

        /// <summary>
        /// Atualiza estoque de um livro
        /// </summary>
        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> UpdateEstoque(int id, [FromBody] int novoEstoque)
        {
            if (novoEstoque < 0)
                return BadRequest("Estoque não pode ser negativo.");

            var sucesso = await _livroRepository.UpdateEstoqueAsync(id, novoEstoque);
            if (!sucesso)
                return NotFound($"Livro com ID {id} não encontrado.");

            return NoContent();
        }

        /// <summary>
        /// Pesquisa livros na base de dados local e, se não encontrar, consulta a API do Google Books
        /// </summary>
        [HttpGet("pesquisar/{termoDeBusca}")]
        public async Task<ActionResult<object>> PesquisarLivros(string termoDeBusca)
        {
            if (string.IsNullOrWhiteSpace(termoDeBusca))
                return BadRequest("Termo de busca não pode ser vazio.");

            try
            {
                // Primeiro, pesquisa na base de dados local
                var livrosLocais = await _livroRepository.SearchByTitleAsync(termoDeBusca);
                
                if (livrosLocais.Any())
                {
                    var livrosLocalDTO = livrosLocais.Select(MapToDTO);
                    return Ok(new
                    {
                        Fonte = "Base de Dados Local",
                        Quantidade = livrosLocalDTO.Count(),
                        Livros = livrosLocalDTO
                    });
                }

                // Se não encontrou na base local, consulta a API do Google Books
                var livrosExternos = await ConsultarGoogleBooksAsync(termoDeBusca);
                
                return Ok(new
                {
                    Fonte = "Google Books API",
                    Quantidade = livrosExternos.Count(),
                    Livros = livrosExternos
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(503, new { 
                    Erro = "Erro ao consultar API externa", 
                    Detalhes = ex.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Erro = "Erro interno do servidor", 
                    Detalhes = ex.Message 
                });
            }
        }

        private async Task<List<LivroPesquisaDTO>> ConsultarGoogleBooksAsync(string termoDeBusca)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            var url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(termoDeBusca)}&maxResults=10";

            try
            {
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                var googleBooksResponse = JsonSerializer.Deserialize<GoogleBooksResponse>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (googleBooksResponse?.Items == null || !googleBooksResponse.Items.Any())
                {
                    return new List<LivroPesquisaDTO>();
                }

                return googleBooksResponse.Items
                    .Where(item => item.VolumeInfo?.Title != null)
                    .Select(item => new LivroPesquisaDTO
                    {
                        Titulo = item.VolumeInfo.Title,
                        Autores = item.VolumeInfo.Authors ?? new List<string>(),
                        ISBN = ExtrairISBN(item.VolumeInfo.IndustryIdentifiers),
                        AnoPublicacao = ExtrairAnoPublicacao(item.VolumeInfo.PublishedDate),
                        Editora = item.VolumeInfo.Publisher,
                        Sinopse = LimitarTexto(item.VolumeInfo.Description, 500),
                        CapaUrl = item.VolumeInfo.ImageLinks?.Thumbnail,
                        Fonte = "Google Books"
                    })
                    .ToList();
            }
            catch (TaskCanceledException)
            {
                throw new HttpRequestException("Timeout na consulta à API do Google Books");
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new HttpRequestException($"Erro ao processar resposta da API: {ex.Message}");
            }
        }

        private static string? ExtrairISBN(List<GoogleBookIndustryIdentifier> identifiers)
        {
            if (identifiers == null || !identifiers.Any())
                return null;

            // Prioriza ISBN_13, depois ISBN_10
            var isbn13 = identifiers.FirstOrDefault(x => x.Type == "ISBN_13")?.Identifier;
            if (!string.IsNullOrEmpty(isbn13))
                return isbn13;

            var isbn10 = identifiers.FirstOrDefault(x => x.Type == "ISBN_10")?.Identifier;
            return isbn10;
        }

        private static int? ExtrairAnoPublicacao(string? publishedDate)
        {
            if (string.IsNullOrEmpty(publishedDate))
                return null;

            // Tenta extrair o ano da data (formato pode variar: "2023", "2023-01", "2023-01-15")
            if (DateTime.TryParse(publishedDate, out var data))
                return data.Year;

            // Se não conseguiu fazer parse da data, tenta extrair apenas os primeiros 4 dígitos
            if (publishedDate.Length >= 4 && int.TryParse(publishedDate[..4], out var ano))
                return ano;

            return null;
        }

        private static string? LimitarTexto(string? texto, int limite)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            if (texto.Length <= limite)
                return texto;

            return texto[..limite] + "...";
        }

        /// <summary>
        /// Exporta todos os livros para um arquivo CSV
        /// </summary>
        [HttpGet("exportar-csv")]
        public async Task<IActionResult> ExportarCSV()
        {
            try
            {
                // Obter todos os livros do repositório
                var livros = await _livroRepository.GetAllAsync();

                // Criar a string CSV usando StreamWriter
                using var memoryStream = new MemoryStream();
                using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

                // Escrever cabeçalhos CSV
                await streamWriter.WriteLineAsync("Id,Titulo,AnoPublicacao");

                // Escrever dados dos livros
                foreach (var livro in livros)
                {
                    var linha = $"{livro.Id},\"{EscaparCsv(livro.Titulo)}\",{livro.AnoPublicacao?.ToString() ?? ""}";
                    await streamWriter.WriteLineAsync(linha);
                }

                await streamWriter.FlushAsync();

                // Retornar o arquivo CSV
                var csvBytes = memoryStream.ToArray();
                return File(csvBytes, "text/csv", "livros.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Erro = "Erro ao gerar arquivo CSV", 
                    Detalhes = ex.Message 
                });
            }
        }

        /// <summary>
        /// Escapa caracteres especiais para formato CSV
        /// </summary>
        private static string EscaparCsv(string? valor)
        {
            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            // Escapa aspas duplas duplicando-as e remove quebras de linha
            return valor.Replace("\"", "\"\"").Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
        }

        private static LivroDTO MapToDTO(Livro livro)
        {
            return new LivroDTO
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                AutorId = livro.AutorId,
                Autor = livro.Autor != null ? new AutorResumoDTO
                {
                    Id = livro.Autor.Id,
                    Nome = livro.Autor.Nome,
                    Nacionalidade = livro.Autor.Nacionalidade
                } : new AutorResumoDTO(),
                ISBN = livro.ISBN,
                AnoPublicacao = livro.AnoPublicacao,
                Editora = livro.Editora,
                Genero = livro.Genero,
                NumeroEdicao = livro.NumeroEdicao,
                NumeroPaginas = livro.NumeroPaginas,
                Idioma = livro.Idioma,
                Sinopse = livro.Sinopse,
                CapaUrl = livro.CapaUrl,
                Preco = livro.Preco,
                EstoqueDisponivel = livro.EstoqueDisponivel,
                EstoqueTotal = livro.EstoqueTotal,
                Ativo = livro.Ativo,
                DataCriacao = livro.DataCriacao,
                DataAtualizacao = livro.DataAtualizacao
            };
        }
    }
}
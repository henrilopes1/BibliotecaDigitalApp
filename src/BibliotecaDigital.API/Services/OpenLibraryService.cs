using BibliotecaDigital.API.DTOs;
using System.Text.Json;
using System.Web;

namespace BibliotecaDigital.API.Services
{
    public interface IOpenLibraryService
    {
        Task<OpenLibrarySearchResponseDto> BuscarLivrosAsync(FiltrosBuscaExternaDto filtros);
        Task<OpenLibraryBookDto?> ObterLivroPorChaveAsync(string chave);
        Task<CapasLivroDto> ObterCapasAsync(string isbn);
        Task<CapasLivroDto> ObterCapasPorIdOpenLibraryAsync(long coverId);
    }

    public class OpenLibraryService : IOpenLibraryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OpenLibraryService> _logger;
        private const string BASE_URL = "https://openlibrary.org";
        private const string COVERS_URL = "https://covers.openlibrary.org/b";

        public OpenLibraryService(HttpClient httpClient, ILogger<OpenLibraryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            // Configurar timeout mais longo para APIs externas
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<OpenLibrarySearchResponseDto> BuscarLivrosAsync(FiltrosBuscaExternaDto filtros)
        {
            try
            {
                var queryParams = ConstruirQueryParametros(filtros);
                var url = $"{BASE_URL}/search.json?{queryParams}";
                
                _logger.LogInformation("Buscando livros na Open Library: {Url}", url);

                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Erro na API da Open Library: {StatusCode}", response.StatusCode);
                    return new OpenLibrarySearchResponseDto();
                }

                var content = await response.Content.ReadAsStringAsync();
                var resultado = JsonSerializer.Deserialize<OpenLibrarySearchResponseDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Encontrados {Count} livros na Open Library", resultado?.NumFound ?? 0);
                return resultado ?? new OpenLibrarySearchResponseDto();
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "Timeout ao buscar livros na Open Library");
                return new OpenLibrarySearchResponseDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar livros na Open Library");
                return new OpenLibrarySearchResponseDto();
            }
        }

        public async Task<OpenLibraryBookDto?> ObterLivroPorChaveAsync(string chave)
        {
            try
            {
                // Remover "/works/" se presente na chave
                var chaveLimpa = chave.Replace("/works/", "").Replace("/books/", "");
                var url = $"{BASE_URL}/works/{chaveLimpa}.json";
                
                _logger.LogInformation("Obtendo detalhes do livro: {Url}", url);

                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Livro não encontrado na Open Library: {Chave}", chave);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var livro = JsonSerializer.Deserialize<OpenLibraryBookDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return livro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter livro por chave: {Chave}", chave);
                return null;
            }
        }

        public async Task<CapasLivroDto> ObterCapasAsync(string isbn)
        {
            try
            {
                _logger.LogInformation("Obtendo capas para ISBN: {ISBN}", isbn);

                var capas = new CapasLivroDto
                {
                    CapaPequena = $"{COVERS_URL}/isbn/{isbn}-S.jpg",
                    CapaMedia = $"{COVERS_URL}/isbn/{isbn}-M.jpg",
                    CapaGrande = $"{COVERS_URL}/isbn/{isbn}-L.jpg"
                };

                // Verificar se pelo menos uma capa existe
                var urlTeste = capas.CapaMedia;
                using var headResponse = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, urlTeste));
                
                if (!headResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Capas não encontradas para ISBN: {ISBN}", isbn);
                    return new CapasLivroDto();
                }

                return capas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter capas para ISBN: {ISBN}", isbn);
                return new CapasLivroDto();
            }
        }

        public async Task<CapasLivroDto> ObterCapasPorIdOpenLibraryAsync(long coverId)
        {
            try
            {
                _logger.LogInformation("Obtendo capas para Cover ID: {CoverId}", coverId);

                var capas = new CapasLivroDto
                {
                    CapaPequena = $"{COVERS_URL}/id/{coverId}-S.jpg",
                    CapaMedia = $"{COVERS_URL}/id/{coverId}-M.jpg",
                    CapaGrande = $"{COVERS_URL}/id/{coverId}-L.jpg"
                };

                // Verificar se a capa existe
                var urlTeste = capas.CapaMedia;
                using var headResponse = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, urlTeste));
                
                if (!headResponse.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Capas não encontradas para Cover ID: {CoverId}", coverId);
                    return new CapasLivroDto();
                }

                return capas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter capas para Cover ID: {CoverId}", coverId);
                return new CapasLivroDto();
            }
        }

        private static string ConstruirQueryParametros(FiltrosBuscaExternaDto filtros)
        {
            var parametros = new List<string>();

            if (!string.IsNullOrEmpty(filtros.Termo))
            {
                parametros.Add($"q={HttpUtility.UrlEncode(filtros.Termo)}");
            }
            else
            {
                var termosBusca = new List<string>();

                if (!string.IsNullOrEmpty(filtros.Titulo))
                    termosBusca.Add($"title:{HttpUtility.UrlEncode(filtros.Titulo)}");

                if (!string.IsNullOrEmpty(filtros.Autor))
                    termosBusca.Add($"author:{HttpUtility.UrlEncode(filtros.Autor)}");

                if (!string.IsNullOrEmpty(filtros.ISBN))
                    termosBusca.Add($"isbn:{filtros.ISBN}");

                if (!string.IsNullOrEmpty(filtros.Categoria))
                    termosBusca.Add($"subject:{HttpUtility.UrlEncode(filtros.Categoria)}");

                if (filtros.AnoMin.HasValue || filtros.AnoMax.HasValue)
                {
                    var anoMin = filtros.AnoMin ?? 1000;
                    var anoMax = filtros.AnoMax ?? DateTime.Now.Year;
                    termosBusca.Add($"first_publish_year:[{anoMin} TO {anoMax}]");
                }

                if (termosBusca.Any())
                {
                    parametros.Add($"q={string.Join(" AND ", termosBusca)}");
                }
                else
                {
                    parametros.Add("q=*"); // Busca geral se nenhum filtro específico
                }
            }

            // Limitar resultados
            parametros.Add($"limit={Math.Min(filtros.Limite, 100)}");

            // Apenas livros com capas se solicitado
            if (filtros.ApenasComCapas)
            {
                parametros.Add("has_fulltext=true");
            }

            // Campos a retornar
            parametros.Add("fields=key,title,author_name,isbn,first_publish_year,subject,number_of_pages_median,language,publisher,ratings_average,ratings_count,has_fulltext,cover_i");

            return string.Join("&", parametros);
        }
    }
}
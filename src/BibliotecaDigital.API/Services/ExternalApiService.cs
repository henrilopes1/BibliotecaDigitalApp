using BibliotecaDigital.API.DTOs;
using System.Text.Json;

namespace BibliotecaDigital.API.Services
{
    public interface IExternalApiService
    {
        Task<CotacaoMoedaDto> GetCotacaoDolarAsync();
        Task<InformacaoTempoDto> GetInformacaoTempoAsync();
        Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency);
        Task<string> GetCurrentTimeInTimezoneAsync(string timezone);
    }

    public partial class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiService> _logger;

        public ExternalApiService(HttpClient httpClient, ILogger<ExternalApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CotacaoMoedaDto> GetCotacaoDolarAsync()
        {
            try
            {
                _logger.LogInformation("Buscando cotação do dólar na API externa");

                // API Frankfurter para cotação de moedas (gratuita)
                var response = await _httpClient.GetAsync("https://api.frankfurter.app/latest?from=USD&to=BRL");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var frankfurterResponse = JsonSerializer.Deserialize<FrankfurterResponseDto>(jsonContent, new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });

                    var cotacao = new CotacaoMoedaDto
                    {
                        Moeda = "USD/BRL",
                        Valor = frankfurterResponse?.Rates?.BRL ?? 5.20m, // Fallback se API falhar
                        DataCotacao = frankfurterResponse?.Date ?? DateTime.UtcNow,
                        Fonte = "Frankfurter API"
                    };

                    _logger.LogInformation("Cotação obtida com sucesso: {Valor}", cotacao.Valor);
                    return cotacao;
                }
                else
                {
                    _logger.LogWarning("API Frankfurter retornou status {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cotação do dólar");
            }

            // Fallback em caso de erro
            return new CotacaoMoedaDto
            {
                Moeda = "USD/BRL",
                Valor = 5.20m,
                DataCotacao = DateTime.UtcNow,
                Fonte = "Valor estimado (API indisponível)"
            };
        }

        public async Task<InformacaoTempoDto> GetInformacaoTempoAsync()
        {
            try
            {
                _logger.LogInformation("Buscando informações de tempo na API externa");

                // API WorldTimeAPI (gratuita) para horário de São Paulo
                var response = await _httpClient.GetAsync("https://worldtimeapi.org/api/timezone/America/Sao_Paulo");
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var worldTimeResponse = JsonSerializer.Deserialize<WorldTimeResponseDto>(jsonContent, new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });

                    var informacaoTempo = new InformacaoTempoDto
                    {
                        DataHora = worldTimeResponse?.Datetime ?? DateTime.UtcNow,
                        Timezone = worldTimeResponse?.Timezone ?? "America/Sao_Paulo",
                        Fonte = "WorldTime API"
                    };

                    _logger.LogInformation("Informação de tempo obtida com sucesso: {DataHora}", informacaoTempo.DataHora);
                    return informacaoTempo;
                }
                else
                {
                    _logger.LogWarning("API WorldTime retornou status {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar informações de tempo");
            }

            // Fallback em caso de erro
            return new InformacaoTempoDto
            {
                DataHora = DateTime.UtcNow,
                Timezone = "UTC",
                Fonte = "Horário local (API indisponível)"
            };
        }
    }

    // DTOs internos para as APIs externas
    internal class FrankfurterResponseDto
    {
        public decimal Amount { get; set; }
        public string Base { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public FrankfurterRatesDto? Rates { get; set; }
    }

    internal class FrankfurterRatesDto
    {
        public decimal BRL { get; set; }
    }

    internal class WorldTimeResponseDto
    {
        public string Abbreviation { get; set; } = string.Empty;
        public string Client_ip { get; set; } = string.Empty;
        public DateTime Datetime { get; set; }
        public int Day_of_week { get; set; }
        public int Day_of_year { get; set; }
        public bool Dst { get; set; }
        public object? Dst_from { get; set; }
        public object? Dst_offset { get; set; }
        public object? Dst_until { get; set; }
        public int Raw_offset { get; set; }
        public string Timezone { get; set; } = string.Empty;
        public long Unixtime { get; set; }
        public string Utc_datetime { get; set; } = string.Empty;
        public string Utc_offset { get; set; } = string.Empty;
        public int Week_number { get; set; }
    }

    // Métodos adicionais para suporte aos endpoints avançados
    public partial class ExternalApiService
    {
        public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency)
        {
            try
            {
                _logger.LogInformation("Buscando taxa de câmbio de {From} para {To}", fromCurrency, toCurrency);

                var response = await _httpClient.GetAsync($"https://api.frankfurter.app/latest?from={fromCurrency}&to={toCurrency}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Erro ao buscar taxa de câmbio: {StatusCode}", response.StatusCode);
                    return 1.0m; // Fallback
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<FrankfurterResponse>(content);

                if (data?.Rates != null && data.Rates.ContainsKey(toCurrency))
                {
                    return data.Rates[toCurrency];
                }

                return 1.0m; // Fallback se não encontrar a moeda
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar taxa de câmbio de {From} para {To}", fromCurrency, toCurrency);
                return 1.0m; // Fallback
            }
        }

        public async Task<string> GetCurrentTimeInTimezoneAsync(string timezone)
        {
            try
            {
                _logger.LogInformation("Buscando horário atual para timezone: {Timezone}", timezone);

                var response = await _httpClient.GetAsync($"http://worldtimeapi.org/api/timezone/{timezone}");
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Erro ao buscar horário: {StatusCode}", response.StatusCode);
                    return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<WorldTimeResponseDto>(content);

                return data?.Datetime.ToString("yyyy-MM-dd HH:mm:ss") ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar horário para timezone: {Timezone}", timezone);
                return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
            }
        }
    }

    // Classes auxiliares para deserialização das APIs
    public class FrankfurterResponse
    {
        public decimal Amount { get; set; }
        public string Base { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public Dictionary<string, decimal> Rates { get; set; } = new();
    }
}
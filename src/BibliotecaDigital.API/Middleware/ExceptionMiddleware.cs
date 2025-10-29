using System.Net;
using System.Text.Json;

namespace BibliotecaDigital.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado capturado pelo middleware: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponseDto();

            switch (exception)
            {
                case ArgumentNullException:
                case ArgumentException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Error = "Dados inválidos";
                    response.Message = exception.Message;
                    break;

                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Error = "Recurso não encontrado";
                    response.Message = exception.Message;
                    break;

                case InvalidOperationException:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Error = "Operação inválida";
                    response.Message = exception.Message;
                    break;

                case UnauthorizedAccessException:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Error = "Acesso negado";
                    response.Message = "Você não tem permissão para acessar este recurso";
                    break;

                case TimeoutException:
                case TaskCanceledException:
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.Error = "Timeout";
                    response.Message = "A requisição demorou muito para ser processada";
                    break;

                case BusinessRuleException businessEx:
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    response.Error = "Regra de negócio violada";
                    response.Message = businessEx.Message;
                    response.Details = businessEx.Details;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Error = "Erro interno do servidor";
                    response.Message = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                    response.Details = exception.Message;
                    break;
            }

            context.Response.StatusCode = response.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorResponseDto
    {
        public int StatusCode { get; set; }
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string TraceId { get; set; } = Guid.NewGuid().ToString();
    }

    // Exceção customizada para regras de negócio
    public class BusinessRuleException : Exception
    {
        public string? Details { get; }

        public BusinessRuleException(string message) : base(message)
        {
        }

        public BusinessRuleException(string message, string details) : base(message)
        {
            Details = details;
        }

        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    // Extension method para registrar o middleware
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
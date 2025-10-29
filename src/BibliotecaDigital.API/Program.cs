using BibliotecaDigital.Data.Context;
using BibliotecaDigital.Data.Repositories;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.API.Services;
using BibliotecaDigital.API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddControllers();

builder.Services.AddDbContext<BibliotecaDigitalContext>(options =>
{
    var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider");
    
    if (databaseProvider == "Oracle")
    {
        var connectionString = builder.Configuration.GetConnectionString("OracleConnection");
        options.UseOracle(connectionString);
        
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    }
    else
    {
        options.UseInMemoryDatabase("BibliotecaDigitalDB");
        
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    }
});

builder.Services.AddScoped<IAutorRepository, AutorRepository>();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();

builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IOpenLibraryService, OpenLibraryService>();
builder.Services.AddScoped<ExternalApiService>();
builder.Services.AddScoped<LivroEnriquecimentoService>();

builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Biblioteca Digital API", 
        Version = "v1",
        Description = "API para gerenciamento de biblioteca digital - Checkpoint FIAP"
    });
});

var app = builder.Build();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Digital API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚀 Biblioteca Digital API iniciada com sucesso!");
logger.LogInformation("📚 Swagger disponível em: http://localhost:5219/swagger");
logger.LogInformation("🗄️  Banco de dados: {DatabaseProvider}", app.Configuration.GetValue<string>("DatabaseProvider"));

app.Run();

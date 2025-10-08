using BibliotecaDigital.Data.Context;
using BibliotecaDigital.Data.Repositories;
using BibliotecaDigital.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Entity Framework Configuration
builder.Services.AddDbContext<BibliotecaDigitalContext>(options =>
{
    // Usar InMemory Database para desenvolvimento
    options.UseInMemoryDatabase("BibliotecaDigitalDB");
    
    // Para Oracle Database (quando credenciais da FIAP estiverem disponíveis):
    // options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});

// Dependency Injection - Repositories
builder.Services.AddScoped<IAutorRepository, AutorRepository>();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();

// HTTP Client Factory
builder.Services.AddHttpClient();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Swagger Configuration
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

// Seed database in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BibliotecaDigitalContext>();
    context.Database.EnsureCreated(); // Garante que o banco seja criado com seed data
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca Digital API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

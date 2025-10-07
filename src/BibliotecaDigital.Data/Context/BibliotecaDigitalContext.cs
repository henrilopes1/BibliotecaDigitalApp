using BibliotecaDigital.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDigital.Data.Context
{
    public class BibliotecaDigitalContext : DbContext
    {
        public BibliotecaDigitalContext(DbContextOptions<BibliotecaDigitalContext> options) : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<PerfilAutor> PerfisAutor { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Autor
            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.DataNascimento);
                entity.Property(e => e.Nacionalidade).HasMaxLength(50);
                entity.Property(e => e.DataCriacao).IsRequired();
                entity.Property(e => e.DataAtualizacao);

                // Relacionamento 1:1 com PerfilAutor
                entity.HasOne(a => a.Perfil)
                      .WithOne(p => p.Autor)
                      .HasForeignKey<PerfilAutor>(p => p.AutorId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relacionamento 1:N com Livro
                entity.HasMany(a => a.Livros)
                      .WithOne(l => l.Autor)
                      .HasForeignKey(l => l.AutorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade PerfilAutor
            modelBuilder.Entity<PerfilAutor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AutorId).IsRequired();
                entity.Property(e => e.Biografia).HasMaxLength(2000);
                entity.Property(e => e.FotoUrl).HasMaxLength(500);
                entity.Property(e => e.Website).HasMaxLength(200);
                entity.Property(e => e.RedesSociais).HasMaxLength(1000);
                entity.Property(e => e.Premios).HasMaxLength(1000);
                entity.Property(e => e.DataCriacao).IsRequired();
                entity.Property(e => e.DataAtualizacao);
            });

            // Configuração da entidade Livro
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.AutorId).IsRequired();
                entity.Property(e => e.ISBN).HasMaxLength(20);
                entity.Property(e => e.AnoPublicacao);
                entity.Property(e => e.Editora).HasMaxLength(100);
                entity.Property(e => e.Genero).HasMaxLength(50);
                entity.Property(e => e.NumeroEdicao);
                entity.Property(e => e.NumeroPaginas);
                entity.Property(e => e.Idioma).HasMaxLength(20);
                entity.Property(e => e.Sinopse).HasMaxLength(2000);
                entity.Property(e => e.CapaUrl).HasMaxLength(500);
                entity.Property(e => e.Preco).HasColumnType("decimal(10,2)");
                entity.Property(e => e.EstoqueDisponivel);
                entity.Property(e => e.EstoqueTotal);
                entity.Property(e => e.Ativo).HasDefaultValue(true);
                entity.Property(e => e.DataCriacao).IsRequired();
                entity.Property(e => e.DataAtualizacao);

                // Relacionamento com Empréstimo
                entity.HasMany(l => l.Emprestimos)
                      .WithOne(e => e.Livro)
                      .HasForeignKey(e => e.LivroId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuração da entidade Emprestimo
            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LivroId).IsRequired();
                entity.Property(e => e.NomeUsuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CpfUsuario).IsRequired().HasMaxLength(14);
                entity.Property(e => e.EmailUsuario).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TelefoneUsuario).HasMaxLength(20);
                entity.Property(e => e.DataEmprestimo).IsRequired();
                entity.Property(e => e.DataDevolucaoPrevista).IsRequired();
                entity.Property(e => e.DataDevolucaoReal);
                entity.Property(e => e.Devolvido).HasDefaultValue(false);
                entity.Property(e => e.MultaAtraso).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Observacoes).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Ativo");
                entity.Property(e => e.DataCriacao).IsRequired();
                entity.Property(e => e.DataAtualizacao);
            });

            // Seed data para desenvolvimento
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Autores
            modelBuilder.Entity<Autor>().HasData(
                new Autor
                {
                    Id = 1,
                    Nome = "Machado de Assis",
                    Email = "machado@literatura.com",
                    DataNascimento = new DateTime(1839, 6, 21),
                    Nacionalidade = "Brasileira",
                    DataCriacao = DateTime.UtcNow
                },
                new Autor
                {
                    Id = 2,
                    Nome = "Clarice Lispector",
                    Email = "clarice@literatura.com",
                    DataNascimento = new DateTime(1920, 12, 10),
                    Nacionalidade = "Brasileira",
                    DataCriacao = DateTime.UtcNow
                }
            );

            // Seed Perfis de Autor
            modelBuilder.Entity<PerfilAutor>().HasData(
                new PerfilAutor
                {
                    Id = 1,
                    AutorId = 1,
                    Biografia = "Joaquim Maria Machado de Assis foi um escritor brasileiro, considerado por muitos críticos, estudiosos, escritores e leitores um dos maiores senão o maior nome da literatura do Brasil.",
                    DataCriacao = DateTime.UtcNow
                },
                new PerfilAutor
                {
                    Id = 2,
                    AutorId = 2,
                    Biografia = "Clarice Lispector foi uma escritora e jornalista brasileira. Nascida na Ucrânia, veio para o Brasil ainda criança. Autora de romances, contos e ensaios, é considerada uma das escritoras brasileiras mais importantes do século XX.",
                    DataCriacao = DateTime.UtcNow
                }
            );

            // Seed Livros
            modelBuilder.Entity<Livro>().HasData(
                new Livro
                {
                    Id = 1,
                    Titulo = "Dom Casmurro",
                    AutorId = 1,
                    AnoPublicacao = 1899,
                    Editora = "Garnier",
                    Genero = "Romance",
                    NumeroEdicao = 1,
                    NumeroPaginas = 256,
                    Idioma = "Português",
                    Sinopse = "Romance narrado em primeira pessoa por Bentinho, que conta a história de seu amor por Capitu desde a infância até o casamento.",
                    Preco = 29.90m,
                    EstoqueDisponivel = 5,
                    EstoqueTotal = 10,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                },
                new Livro
                {
                    Id = 2,
                    Titulo = "A Hora da Estrela",
                    AutorId = 2,
                    AnoPublicacao = 1977,
                    Editora = "José Olympio",
                    Genero = "Romance",
                    NumeroEdicao = 1,
                    NumeroPaginas = 87,
                    Idioma = "Português",
                    Sinopse = "A história de Macabéa, uma jovem alagoana que vive no Rio de Janeiro, contada por Rodrigo S.M.",
                    Preco = 24.90m,
                    EstoqueDisponivel = 3,
                    EstoqueTotal = 8,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                }
            );
        }
    }
}
using BibliotecaDigital.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDigital.Data.Context
{
    public class BibliotecaDigitalContext : DbContext
    {
        public BibliotecaDigitalContext(DbContextOptions<BibliotecaDigitalContext> options) : base(options)
        {
        }

        // DbSets - Representam as tabelas no banco
        public DbSet<Autor> Autores { get; set; }
        public DbSet<PerfilAutor> PerfisAutor { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.ToTable("AUTORES");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.Nome).HasColumnName("NOME").IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(255);
                entity.Property(e => e.DataNascimento).HasColumnName("DATANASCIMENTO");
                entity.Property(e => e.Nacionalidade).HasColumnName("NACIONALIDADE").HasMaxLength(100);
                entity.Property(e => e.DataCriacao).HasColumnName("CREATEDAT").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("UPDATEDAT");

                entity.HasOne(a => a.Perfil)
                      .WithOne(p => p.Autor)
                      .HasForeignKey<PerfilAutor>(p => p.AutorId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(a => a.Livros)
                      .WithOne(l => l.Autor)
                      .HasForeignKey(l => l.AutorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PerfilAutor>(entity =>
            {
                entity.ToTable("PERFILAUTORES");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.AutorId).HasColumnName("AUTORID").IsRequired();
                entity.Property(e => e.Biografia).HasColumnName("BIOGRAFIA");
                entity.Property(e => e.FotoUrl).HasColumnName("FOTOURL").HasMaxLength(500);
                entity.Property(e => e.Website).HasColumnName("WEBSITE").HasMaxLength(300);
                entity.Property(e => e.RedesSociais).HasColumnName("REDESSOCIAIS").HasMaxLength(1000);
                entity.Property(e => e.Premios).HasColumnName("PREMIOS").HasMaxLength(2000);
                entity.Property(e => e.DataCriacao).HasColumnName("CREATEDAT").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("UPDATEDAT");
                
                entity.HasIndex(e => e.AutorId).IsUnique();
            });

            modelBuilder.Entity<Livro>(entity =>
            {
                entity.ToTable("LIVROS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.Titulo).HasColumnName("TITULO").IsRequired().HasMaxLength(300);
                entity.Property(e => e.AutorId).HasColumnName("AUTORID").IsRequired();
                entity.Property(e => e.ISBN).HasColumnName("ISBN").HasMaxLength(17);
                entity.Property(e => e.AnoPublicacao).HasColumnName("ANOPUBLICACAO");
                entity.Property(e => e.Editora).HasColumnName("EDITORA").HasMaxLength(200);
                entity.Property(e => e.Genero).HasColumnName("GENERO").HasMaxLength(100);
                entity.Property(e => e.NumeroPaginas).HasColumnName("PAGINAS");
                entity.Property(e => e.Idioma).HasColumnName("IDIOMA").HasMaxLength(50);
                entity.Property(e => e.Sinopse).HasColumnName("DESCRICAO");
                entity.Property(e => e.CapaUrl).HasColumnName("CAPAURL").HasMaxLength(500);
                entity.Property(e => e.Preco).HasColumnName("PRECO").HasColumnType("NUMBER(10,2)");
                entity.Property(e => e.EstoqueDisponivel).HasColumnName("ESTOQUE");
                entity.Property(e => e.Ativo)
                    .HasColumnName("DISPONIVEL")
                    .HasDefaultValue(1)
                    .HasConversion(
                        v => v ? 1 : 0,
                        v => v == 1
                    );
                entity.Property(e => e.DataCriacao).HasColumnName("CREATEDAT").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("UPDATEDAT");

                entity.HasMany(l => l.Emprestimos)
                      .WithOne(e => e.Livro)
                      .HasForeignKey(e => e.LivroId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasIndex(e => e.AutorId);
                entity.HasIndex(e => e.ISBN);
            });

            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.ToTable("EMPRESTIMOS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedOnAdd();
                entity.Property(e => e.LivroId).HasColumnName("LIVROID").IsRequired();
                entity.Property(e => e.NomeUsuario).HasColumnName("NOMEUSUARIO").IsRequired().HasMaxLength(200);
                entity.Property(e => e.EmailUsuario).HasColumnName("EMAILUSUARIO").IsRequired().HasMaxLength(255);
                entity.Property(e => e.TelefoneUsuario).HasColumnName("TELEFONEUSUARIO").HasMaxLength(20);
                entity.Property(e => e.DataEmprestimo).HasColumnName("DATAEMPRESTIMO").IsRequired();
                entity.Property(e => e.DataDevolucaoPrevista).HasColumnName("DATAPREVISTADEVOLUCAO").IsRequired();
                entity.Property(e => e.DataDevolucaoReal).HasColumnName("DATADEVOLUCAO");
                entity.Property(e => e.MultaAtraso).HasColumnName("MULTACALCULADA").HasColumnType("NUMBER(10,2)");
                entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES").HasMaxLength(1000);
                entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("Ativo");
                entity.Property(e => e.DataCriacao).HasColumnName("CREATEDAT").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("UPDATEDAT");
                
                entity.HasIndex(e => e.LivroId);
            });
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Autores
            modelBuilder.Entity<Autor>().HasData(
                new Autor
                {
                    Id = 1,
                    Nome = "Machado de Assis",
                    Email = "machado@literatura.com.br",
                    DataNascimento = new DateTime(1839, 6, 21),
                    Nacionalidade = "Brasileira",
                    DataCriacao = DateTime.UtcNow
                },
                new Autor
                {
                    Id = 2,
                    Nome = "Clarice Lispector",
                    Email = "clarice@literatura.com.br",
                    DataNascimento = new DateTime(1920, 12, 10),
                    Nacionalidade = "Brasileira",
                    DataCriacao = DateTime.UtcNow
                },
                new Autor
                {
                    Id = 3,
                    Nome = "Jorge Amado",
                    Email = "jorge@literatura.com.br",
                    DataNascimento = new DateTime(1912, 8, 10),
                    Nacionalidade = "Brasileira",
                    DataCriacao = DateTime.UtcNow
                }
            );

            // Seed Perfis de Autor (Relação 1:1)
            modelBuilder.Entity<PerfilAutor>().HasData(
                new PerfilAutor
                {
                    Id = 1,
                    AutorId = 1,
                    Biografia = "Joaquim Maria Machado de Assis foi um escritor brasileiro, considerado por muitos críticos o maior nome da literatura brasileira. Fundador da Academia Brasileira de Letras.",
                    Website = "https://machadodeassis.org.br",
                    Premios = "Fundador da Academia Brasileira de Letras",
                    DataCriacao = DateTime.UtcNow
                },
                new PerfilAutor
                {
                    Id = 2,
                    AutorId = 2,
                    Biografia = "Clarice Lispector foi uma escritora e jornalista brasileira. Nascida na Ucrânia, considerada uma das escritoras brasileiras mais importantes do século XX.",
                    Premios = "Prêmio Jabuti, Prêmio Fundação Cultural do Distrito Federal",
                    DataCriacao = DateTime.UtcNow
                },
                new PerfilAutor
                {
                    Id = 3,
                    AutorId = 3,
                    Biografia = "Jorge Amado foi um dos mais famosos e traduzidos escritores brasileiros de todos os tempos. Suas obras foram adaptadas para cinema, televisão e teatro.",
                    Premios = "Prêmio Stalin da Paz, Prêmio Jabuti",
                    DataCriacao = DateTime.UtcNow
                }
            );

            // Seed Livros (Relação 1:N com Autor)
            modelBuilder.Entity<Livro>().HasData(
                new Livro
                {
                    Id = 1,
                    Titulo = "Dom Casmurro",
                    AutorId = 1,
                    ISBN = "978-85-254-0001-1",
                    AnoPublicacao = 1899,
                    Editora = "Garnier",
                    Genero = "Romance",
                    NumeroEdicao = 1,
                    NumeroPaginas = 256,
                    Idioma = "Português",
                    Sinopse = "Romance narrado em primeira pessoa por Bentinho, que conta a história de seu amor por Capitu desde a infância até o casamento.",
                    Preco = 29.90m,
                    EstoqueDisponivel = 15,
                    EstoqueTotal = 20,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                },
                new Livro
                {
                    Id = 2,
                    Titulo = "O Cortiço",
                    AutorId = 1,
                    ISBN = "978-85-254-0002-8",
                    AnoPublicacao = 1890,
                    Editora = "Garnier",
                    Genero = "Romance",
                    NumeroEdicao = 1,
                    NumeroPaginas = 312,
                    Idioma = "Português",
                    Sinopse = "Romance naturalista que retrata a vida em um cortiço no Rio de Janeiro do século XIX.",
                    Preco = 27.90m,
                    EstoqueDisponivel = 8,
                    EstoqueTotal = 15,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                },
                new Livro
                {
                    Id = 3,
                    Titulo = "A Hora da Estrela",
                    AutorId = 2,
                    ISBN = "978-85-254-0003-5",
                    AnoPublicacao = 1977,
                    Editora = "José Olympio",
                    Genero = "Romance",
                    NumeroEdicao = 1,
                    NumeroPaginas = 87,
                    Idioma = "Português",
                    Sinopse = "A história de Macabéa, uma jovem alagoana que vive no Rio de Janeiro, contada por Rodrigo S.M.",
                    Preco = 24.90m,
                    EstoqueDisponivel = 12,
                    EstoqueTotal = 18,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                },
                new Livro
                {
                    Id = 4,
                    Titulo = "Gabriela, Cravo e Canela",
                    AutorId = 3,
                    ISBN = "978-85-254-0004-2",
                    AnoPublicacao = 1958,
                    Editora = "Martins",
                    Genero = "Romance",
                    NumeroEdicao = 1,
                    NumeroPaginas = 421,
                    Idioma = "Português",
                    Sinopse = "Romance que retrata a vida na cidade de Ilhéus nos anos 1920, com foco na personagem Gabriela.",
                    Preco = 32.90m,
                    EstoqueDisponivel = 6,
                    EstoqueTotal = 12,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                }
            );

            // Seed Empréstimos (Relação 1:N com Livro)
            modelBuilder.Entity<Emprestimo>().HasData(
                new Emprestimo
                {
                    Id = 1,
                    LivroId = 1,
                    NomeUsuario = "João Silva",
                    CpfUsuario = "123.456.789-01",
                    EmailUsuario = "joao.silva@email.com",
                    TelefoneUsuario = "(11) 98765-4321",
                    DataEmprestimo = DateTime.UtcNow.AddDays(-10),
                    DataDevolucaoPrevista = DateTime.UtcNow.AddDays(5),
                    // Devolvido é calculado automaticamente
                    Status = "Ativo",
                    DataCriacao = DateTime.UtcNow
                },
                new Emprestimo
                {
                    Id = 2,
                    LivroId = 3,
                    NomeUsuario = "Maria Santos",
                    CpfUsuario = "987.654.321-09",
                    EmailUsuario = "maria.santos@email.com",
                    TelefoneUsuario = "(11) 91234-5678",
                    DataEmprestimo = DateTime.UtcNow.AddDays(-5),
                    DataDevolucaoPrevista = DateTime.UtcNow.AddDays(10),
                    // Devolvido é calculado automaticamente
                    Status = "Ativo",
                    DataCriacao = DateTime.UtcNow
                },
                new Emprestimo
                {
                    Id = 3,
                    LivroId = 2,
                    NomeUsuario = "Pedro Oliveira",
                    CpfUsuario = "111.222.333-44",
                    EmailUsuario = "pedro.oliveira@email.com",
                    TelefoneUsuario = "(11) 95555-1111",
                    DataEmprestimo = DateTime.UtcNow.AddDays(-20),
                    DataDevolucaoPrevista = DateTime.UtcNow.AddDays(-5),
                    DataDevolucaoReal = DateTime.UtcNow.AddDays(-3), // Com data = Devolvido fica true
                    // Devolvido é calculado automaticamente
                    MultaAtraso = 5.00m,
                    Status = "Finalizado",
                    Observacoes = "Devolvido com 2 dias de atraso",
                    DataCriacao = DateTime.UtcNow
                }
            );
        }
    }
}
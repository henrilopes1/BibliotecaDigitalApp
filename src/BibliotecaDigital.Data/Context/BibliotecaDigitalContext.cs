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

            // Configuração da entidade Autor
            modelBuilder.Entity<Autor>(entity =>
            {
                entity.ToTable("TB_AUTORES"); // Nome da tabela para Oracle
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID_AUTOR").ValueGeneratedOnAdd();
                entity.Property(e => e.Nome).HasColumnName("NOME").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(100);
                entity.Property(e => e.DataNascimento).HasColumnName("DATA_NASCIMENTO");
                entity.Property(e => e.Nacionalidade).HasColumnName("NACIONALIDADE").HasMaxLength(50);
                entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO");

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

            // Configuração da entidade PerfilAutor (Relação 1:1)
            modelBuilder.Entity<PerfilAutor>(entity =>
            {
                entity.ToTable("TB_PERFIS_AUTOR");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID_PERFIL").ValueGeneratedOnAdd();
                entity.Property(e => e.AutorId).HasColumnName("ID_AUTOR").IsRequired();
                entity.Property(e => e.Biografia).HasColumnName("BIOGRAFIA").HasMaxLength(2000);
                entity.Property(e => e.FotoUrl).HasColumnName("FOTO_URL").HasMaxLength(500);
                entity.Property(e => e.Website).HasColumnName("WEBSITE").HasMaxLength(200);
                entity.Property(e => e.RedesSociais).HasColumnName("REDES_SOCIAIS").HasMaxLength(1000);
                entity.Property(e => e.Premios).HasColumnName("PREMIOS").HasMaxLength(1000);
                entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO");
                
                // Índice único para garantir relação 1:1
                entity.HasIndex(e => e.AutorId).IsUnique();
            });

            // Configuração da entidade Livro (Relação 1:N com Autor)
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.ToTable("TB_LIVROS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID_LIVRO").ValueGeneratedOnAdd();
                entity.Property(e => e.Titulo).HasColumnName("TITULO").IsRequired().HasMaxLength(200);
                entity.Property(e => e.AutorId).HasColumnName("ID_AUTOR").IsRequired();
                entity.Property(e => e.ISBN).HasColumnName("ISBN").HasMaxLength(20);
                entity.Property(e => e.AnoPublicacao).HasColumnName("ANO_PUBLICACAO");
                entity.Property(e => e.Editora).HasColumnName("EDITORA").HasMaxLength(100);
                entity.Property(e => e.Genero).HasColumnName("GENERO").HasMaxLength(50);
                entity.Property(e => e.NumeroEdicao).HasColumnName("NUMERO_EDICAO");
                entity.Property(e => e.NumeroPaginas).HasColumnName("NUMERO_PAGINAS");
                entity.Property(e => e.Idioma).HasColumnName("IDIOMA").HasMaxLength(20);
                entity.Property(e => e.Sinopse).HasColumnName("SINOPSE").HasMaxLength(2000);
                entity.Property(e => e.CapaUrl).HasColumnName("CAPA_URL").HasMaxLength(500);
                entity.Property(e => e.Preco).HasColumnName("PRECO").HasColumnType("NUMBER(10,2)");
                entity.Property(e => e.EstoqueDisponivel).HasColumnName("ESTOQUE_DISPONIVEL");
                entity.Property(e => e.EstoqueTotal).HasColumnName("ESTOQUE_TOTAL");
                entity.Property(e => e.Ativo).HasColumnName("ATIVO").HasDefaultValue(1);
                entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO");

                // Relacionamento com Empréstimo (1:N)
                entity.HasMany(l => l.Emprestimos)
                      .WithOne(e => e.Livro)
                      .HasForeignKey(e => e.LivroId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Índice para melhorar performance
                entity.HasIndex(e => e.AutorId);
                entity.HasIndex(e => e.ISBN);
            });

            // Configuração da entidade Emprestimo (Relação 1:N com Livro)
            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.ToTable("TB_EMPRESTIMOS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID_EMPRESTIMO").ValueGeneratedOnAdd();
                entity.Property(e => e.LivroId).HasColumnName("ID_LIVRO").IsRequired();
                entity.Property(e => e.NomeUsuario).HasColumnName("NOME_USUARIO").IsRequired().HasMaxLength(100);
                entity.Property(e => e.CpfUsuario).HasColumnName("CPF_USUARIO").IsRequired().HasMaxLength(14);
                entity.Property(e => e.EmailUsuario).HasColumnName("EMAIL_USUARIO").IsRequired().HasMaxLength(100);
                entity.Property(e => e.TelefoneUsuario).HasColumnName("TELEFONE_USUARIO").HasMaxLength(20);
                entity.Property(e => e.DataEmprestimo).HasColumnName("DATA_EMPRESTIMO").IsRequired();
                entity.Property(e => e.DataDevolucaoPrevista).HasColumnName("DATA_DEVOLUCAO_PREVISTA").IsRequired();
                entity.Property(e => e.DataDevolucaoReal).HasColumnName("DATA_DEVOLUCAO_REAL");
                entity.Property(e => e.Devolvido).HasColumnName("DEVOLVIDO").HasDefaultValue(0);
                entity.Property(e => e.MultaAtraso).HasColumnName("MULTA_ATRASO").HasColumnType("NUMBER(10,2)");
                entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES").HasMaxLength(500);
                entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("Ativo");
                entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").IsRequired();
                entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO");
                
                // Índices para melhorar performance
                entity.HasIndex(e => e.LivroId);
                entity.HasIndex(e => e.CpfUsuario);
            });

            // Seed data para demonstração da API
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
                    Devolvido = false,
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
                    Devolvido = false,
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
                    DataDevolucaoReal = DateTime.UtcNow.AddDays(-3),
                    Devolvido = true,
                    MultaAtraso = 5.00m,
                    Status = "Finalizado",
                    Observacoes = "Devolvido com 2 dias de atraso",
                    DataCriacao = DateTime.UtcNow
                }
            );
        }
    }
}
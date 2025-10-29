using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaDigital.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_AUTORES",
                columns: table => new
                {
                    ID_AUTOR = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    DATA_NASCIMENTO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    NACIONALIDADE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AUTORES", x => x.ID_AUTOR);
                });

            migrationBuilder.CreateTable(
                name: "TB_LIVROS",
                columns: table => new
                {
                    ID_LIVRO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TITULO = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    ID_AUTOR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ISBN = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    ANO_PUBLICACAO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    EDITORA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: true),
                    GENERO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    NUMERO_EDICAO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    NUMERO_PAGINAS = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IDIOMA = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    SINOPSE = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    CAPA_URL = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    PRECO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                    ESTOQUE_DISPONIVEL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ESTOQUE_TOTAL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ATIVO = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_LIVROS", x => x.ID_LIVRO);
                    table.ForeignKey(
                        name: "FK_TB_LIVROS_TB_AUTORES_ID_AUTOR",
                        column: x => x.ID_AUTOR,
                        principalTable: "TB_AUTORES",
                        principalColumn: "ID_AUTOR",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_PERFIS_AUTOR",
                columns: table => new
                {
                    ID_PERFIL = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_AUTOR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    BIOGRAFIA = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: true),
                    FOTO_URL = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    WEBSITE = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    REDES_SOCIAIS = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    PREMIOS = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PERFIS_AUTOR", x => x.ID_PERFIL);
                    table.ForeignKey(
                        name: "FK_TB_PERFIS_AUTOR_TB_AUTORES_ID_AUTOR",
                        column: x => x.ID_AUTOR,
                        principalTable: "TB_AUTORES",
                        principalColumn: "ID_AUTOR",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_EMPRESTIMOS",
                columns: table => new
                {
                    ID_EMPRESTIMO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_LIVRO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NOME_USUARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CPF_USUARIO = table.Column<string>(type: "NVARCHAR2(14)", maxLength: 14, nullable: false),
                    EMAIL_USUARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    TELEFONE_USUARIO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    DATA_EMPRESTIMO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_DEVOLUCAO_PREVISTA = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_DEVOLUCAO_REAL = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DEVOLVIDO = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                    MULTA_ATRASO = table.Column<decimal>(type: "NUMBER(10,2)", nullable: true),
                    OBSERVACOES = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    STATUS = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false, defaultValue: "Ativo"),
                    DATA_CRIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DATA_ATUALIZACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EMPRESTIMOS", x => x.ID_EMPRESTIMO);
                    table.ForeignKey(
                        name: "FK_TB_EMPRESTIMOS_TB_LIVROS_ID_LIVRO",
                        column: x => x.ID_LIVRO,
                        principalTable: "TB_LIVROS",
                        principalColumn: "ID_LIVRO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TB_AUTORES",
                columns: new[] { "ID_AUTOR", "DATA_ATUALIZACAO", "DATA_CRIACAO", "DATA_NASCIMENTO", "EMAIL", "NACIONALIDADE", "NOME" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(1932), new DateTime(1839, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "machado@literatura.com.br", "Brasileira", "Machado de Assis" },
                    { 2, null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(1935), new DateTime(1920, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "clarice@literatura.com.br", "Brasileira", "Clarice Lispector" },
                    { 3, null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(1937), new DateTime(1912, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "jorge@literatura.com.br", "Brasileira", "Jorge Amado" }
                });

            migrationBuilder.InsertData(
                table: "TB_LIVROS",
                columns: new[] { "ID_LIVRO", "ANO_PUBLICACAO", "ATIVO", "ID_AUTOR", "CAPA_URL", "DATA_ATUALIZACAO", "DATA_CRIACAO", "EDITORA", "ESTOQUE_DISPONIVEL", "ESTOQUE_TOTAL", "GENERO", "ISBN", "IDIOMA", "NUMERO_EDICAO", "NUMERO_PAGINAS", "PRECO", "SINOPSE", "TITULO" },
                values: new object[,]
                {
                    { 1, 1899, true, 1, null, null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2126), "Garnier", 15, 20, "Romance", "978-85-254-0001-1", "Português", 1, 256, 29.90m, "Romance narrado em primeira pessoa por Bentinho, que conta a história de seu amor por Capitu desde a infância até o casamento.", "Dom Casmurro" },
                    { 2, 1890, true, 1, null, null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2129), "Garnier", 8, 15, "Romance", "978-85-254-0002-8", "Português", 1, 312, 27.90m, "Romance naturalista que retrata a vida em um cortiço no Rio de Janeiro do século XIX.", "O Cortiço" },
                    { 3, 1977, true, 2, null, null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2133), "José Olympio", 12, 18, "Romance", "978-85-254-0003-5", "Português", 1, 87, 24.90m, "A história de Macabéa, uma jovem alagoana que vive no Rio de Janeiro, contada por Rodrigo S.M.", "A Hora da Estrela" },
                    { 4, 1958, true, 3, null, null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2135), "Martins", 6, 12, "Romance", "978-85-254-0004-2", "Português", 1, 421, 32.90m, "Romance que retrata a vida na cidade de Ilhéus nos anos 1920, com foco na personagem Gabriela.", "Gabriela, Cravo e Canela" }
                });

            migrationBuilder.InsertData(
                table: "TB_PERFIS_AUTOR",
                columns: new[] { "ID_PERFIL", "ID_AUTOR", "BIOGRAFIA", "DATA_ATUALIZACAO", "DATA_CRIACAO", "FOTO_URL", "PREMIOS", "REDES_SOCIAIS", "WEBSITE" },
                values: new object[,]
                {
                    { 1, 1, "Joaquim Maria Machado de Assis foi um escritor brasileiro, considerado por muitos críticos o maior nome da literatura brasileira. Fundador da Academia Brasileira de Letras.", null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2090), null, "Fundador da Academia Brasileira de Letras", null, "https://machadodeassis.org.br" },
                    { 2, 2, "Clarice Lispector foi uma escritora e jornalista brasileira. Nascida na Ucrânia, considerada uma das escritoras brasileiras mais importantes do século XX.", null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2092), null, "Prêmio Jabuti, Prêmio Fundação Cultural do Distrito Federal", null, null },
                    { 3, 3, "Jorge Amado foi um dos mais famosos e traduzidos escritores brasileiros de todos os tempos. Suas obras foram adaptadas para cinema, televisão e teatro.", null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2093), null, "Prêmio Stalin da Paz, Prêmio Jabuti", null, null }
                });

            migrationBuilder.InsertData(
                table: "TB_EMPRESTIMOS",
                columns: new[] { "ID_EMPRESTIMO", "CPF_USUARIO", "DATA_ATUALIZACAO", "DATA_CRIACAO", "DATA_DEVOLUCAO_PREVISTA", "DATA_DEVOLUCAO_REAL", "DATA_EMPRESTIMO", "EMAIL_USUARIO", "ID_LIVRO", "MULTA_ATRASO", "NOME_USUARIO", "OBSERVACOES", "STATUS", "TELEFONE_USUARIO" },
                values: new object[,]
                {
                    { 1, "123.456.789-01", null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2171), new DateTime(2025, 11, 2, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2170), null, new DateTime(2025, 10, 18, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2165), "joao.silva@email.com", 1, null, "João Silva", null, "Ativo", "(11) 98765-4321" },
                    { 2, "987.654.321-09", null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2174), new DateTime(2025, 11, 7, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2173), null, new DateTime(2025, 10, 23, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2173), "maria.santos@email.com", 3, null, "Maria Santos", null, "Ativo", "(11) 91234-5678" }
                });

            migrationBuilder.InsertData(
                table: "TB_EMPRESTIMOS",
                columns: new[] { "ID_EMPRESTIMO", "CPF_USUARIO", "DATA_ATUALIZACAO", "DATA_CRIACAO", "DATA_DEVOLUCAO_PREVISTA", "DATA_DEVOLUCAO_REAL", "DATA_EMPRESTIMO", "DEVOLVIDO", "EMAIL_USUARIO", "ID_LIVRO", "MULTA_ATRASO", "NOME_USUARIO", "OBSERVACOES", "STATUS", "TELEFONE_USUARIO" },
                values: new object[] { 3, "111.222.333-44", null, new DateTime(2025, 10, 28, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2178), new DateTime(2025, 10, 23, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2176), new DateTime(2025, 10, 25, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2176), new DateTime(2025, 10, 8, 15, 20, 42, 550, DateTimeKind.Utc).AddTicks(2175), true, "pedro.oliveira@email.com", 2, 5.00m, "Pedro Oliveira", "Devolvido com 2 dias de atraso", "Finalizado", "(11) 95555-1111" });

            migrationBuilder.CreateIndex(
                name: "IX_TB_EMPRESTIMOS_CPF_USUARIO",
                table: "TB_EMPRESTIMOS",
                column: "CPF_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_EMPRESTIMOS_ID_LIVRO",
                table: "TB_EMPRESTIMOS",
                column: "ID_LIVRO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LIVROS_ID_AUTOR",
                table: "TB_LIVROS",
                column: "ID_AUTOR");

            migrationBuilder.CreateIndex(
                name: "IX_TB_LIVROS_ISBN",
                table: "TB_LIVROS",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_TB_PERFIS_AUTOR_ID_AUTOR",
                table: "TB_PERFIS_AUTOR",
                column: "ID_AUTOR",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_EMPRESTIMOS");

            migrationBuilder.DropTable(
                name: "TB_PERFIS_AUTOR");

            migrationBuilder.DropTable(
                name: "TB_LIVROS");

            migrationBuilder.DropTable(
                name: "TB_AUTORES");
        }
    }
}

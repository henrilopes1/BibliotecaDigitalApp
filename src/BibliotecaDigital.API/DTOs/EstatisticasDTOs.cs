using System.ComponentModel.DataAnnotations;

namespace BibliotecaDigital.API.DTOs
{
    // DTOs para Estatísticas e Dashboard
    public class EstatisticasDto
    {
        public int TotalAutores { get; set; }
        public int TotalLivros { get; set; }
        public int TotalEmprestimos { get; set; }
        public int EmprestimosAtivos { get; set; }
        public int EmprestimosVencidos { get; set; }
        public decimal MultasTotais { get; set; }
        public List<AutorMaisPopularDto> AutoresMaisPopulares { get; set; } = new();
        public List<LivroMaisEmprestadoDto> LivrosMaisEmprestados { get; set; } = new();
        public List<EstatisticaMensalDto> EstatisticasMensais { get; set; } = new();
    }

    public class RelatorioCompletoDto
    {
        public EstatisticasDto Estatisticas { get; set; } = new();
        public CotacaoMoedaDto CotacaoDolar { get; set; } = new();
        public InformacaoTempoDto InformacaoTempo { get; set; } = new();
        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;
    }

    public class DashboardDto
    {
        public EstatisticasDto Resumo { get; set; } = new();
        public List<EmprestimoRecenteDto> EmprestimosRecentes { get; set; } = new();
        public List<LivroEstoqueBaixoDto> LivrosEstoqueBaixo { get; set; } = new();
        public AlertasDto Alertas { get; set; } = new();
        public int TotalPaginas { get; set; }
        public int PaginaAtual { get; set; }
    }

    public class AutorMaisPopularDto
    {
        public int AutorId { get; set; }
        public string NomeAutor { get; set; } = string.Empty;
        public int TotalEmprestimos { get; set; }
        public int TotalLivros { get; set; }
    }

    public class LivroMaisEmprestadoDto
    {
        public int LivroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string NomeAutor { get; set; } = string.Empty;
        public int TotalEmprestimos { get; set; }
        public decimal? Preco { get; set; }
    }

    public class EstatisticaMensalDto
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string MesNome { get; set; } = string.Empty;
        public int TotalEmprestimos { get; set; }
        public int TotalDevolucoes { get; set; }
        public decimal MultasArrecadadas { get; set; }
    }

    public class EmprestimoRecenteDto
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string LivroTitulo { get; set; } = string.Empty;
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public bool Devolvido { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class LivroEstoqueBaixoDto
    {
        public int LivroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string NomeAutor { get; set; } = string.Empty;
        public int EstoqueDisponivel { get; set; }
        public int EstoqueTotal { get; set; }
        public string AlertaLevel { get; set; } = string.Empty; // "Crítico", "Baixo", "Atenção"
    }

    public class AlertasDto
    {
        public int EmprestimosVencidos { get; set; }
        public int EmprestimosVencendoHoje { get; set; }
        public int LivrosEstoqueCritico { get; set; }
        public int LivrosEstoqueBaixo { get; set; }
        public decimal MultasPendentes { get; set; }
    }

    public class CotacaoMoedaDto
    {
        public string Moeda { get; set; } = "USD";
        public decimal Valor { get; set; }
        public DateTime DataCotacao { get; set; }
        public string Fonte { get; set; } = string.Empty;
    }

    public class InformacaoTempoDto
    {
        public DateTime DataHora { get; set; }
        public string Timezone { get; set; } = string.Empty;
        public string Fonte { get; set; } = string.Empty;
    }

    public class FiltrosDashboardDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Página deve ser maior que 0")]
        public int Pagina { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
        public int TamanhoPagina { get; set; } = 10;

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public string? AutorNome { get; set; }
        public string? LivroTitulo { get; set; }
        public string? StatusEmprestimo { get; set; }

        [RegularExpression("^(nome|data|popularidade|emprestimos)$", ErrorMessage = "Ordenação deve ser: nome, data, popularidade ou emprestimos")]
        public string OrdenarPor { get; set; } = "data";

        [RegularExpression("^(asc|desc)$", ErrorMessage = "Direção deve ser: asc ou desc")]
        public string Direcao { get; set; } = "desc";

        public bool IncluirVencidos { get; set; } = true;
        public bool IncluirAtivos { get; set; } = true;
        public bool IncluirDevolvidos { get; set; } = false;
    }
}
using System.ComponentModel.DataAnnotations;

namespace BibliotecaDigital.API.DTOs
{
    // DTOs para Emprestimo
    public class EmprestimoDTO
    {
        public int Id { get; set; }
        public int LivroId { get; set; }
        public LivroResumoDTO Livro { get; set; } = new();
        public string NomeUsuario { get; set; } = string.Empty;
        public string CpfUsuario { get; set; } = string.Empty;
        public string EmailUsuario { get; set; } = string.Empty;
        public string? TelefoneUsuario { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
        public bool Devolvido { get; set; }
        public decimal? MultaAtraso { get; set; }
        public string? Observacoes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class CreateEmprestimoDTO
    {
        [Required(ErrorMessage = "LivroId é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "LivroId deve ser um número positivo")]
        public int LivroId { get; set; }

        [Required(ErrorMessage = "Nome do usuário é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome do usuário deve ter no máximo 100 caracteres")]
        public string NomeUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "CPF do usuário é obrigatório")]
        [MaxLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$", ErrorMessage = "CPF deve estar no formato 000.000.000-00 ou 00000000000")]
        public string CpfUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email do usuário é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [MaxLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string EmailUsuario { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        [Phone(ErrorMessage = "Telefone deve ter um formato válido")]
        public string? TelefoneUsuario { get; set; }

        [MaxLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }

    public class UpdateEmprestimoDTO
    {
        [Required(ErrorMessage = "Nome do usuário é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome do usuário deve ter no máximo 100 caracteres")]
        public string NomeUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email do usuário é obrigatório")]
        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [MaxLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string EmailUsuario { get; set; } = string.Empty;

        [MaxLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        [Phone(ErrorMessage = "Telefone deve ter um formato válido")]
        public string? TelefoneUsuario { get; set; }

        [MaxLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }

    public class DevolucaoDTO
    {
        [Required(ErrorMessage = "Data de devolução é obrigatória")]
        public DateTime DataDevolucao { get; set; } = DateTime.UtcNow;

        [MaxLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
        public string? Observacoes { get; set; }
    }

    public class EmprestimoResumoDTO
    {
        public int Id { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string CpfUsuario { get; set; } = string.Empty;
        public string LivroTitulo { get; set; } = string.Empty;
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucaoPrevista { get; set; }
        public bool Devolvido { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? MultaAtraso { get; set; }
    }
}
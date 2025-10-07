using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaDigital.Domain.Models
{
    public class Emprestimo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int LivroId { get; set; }
        
        [ForeignKey("LivroId")]
        public virtual Livro Livro { get; set; } = null!;
        
        [Required]
        [MaxLength(100)]
        public string NomeUsuario { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(14)]
        public string CpfUsuario { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string EmailUsuario { get; set; } = string.Empty;
        
        [MaxLength(20)]
        public string? TelefoneUsuario { get; set; }
        
        [Required]
        public DateTime DataEmprestimo { get; set; }
        
        [Required]
        public DateTime DataDevolucaoPrevista { get; set; }
        
        public DateTime? DataDevolucaoReal { get; set; }
        
        public bool Devolvido { get; set; } = false;
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? MultaAtraso { get; set; }
        
        [MaxLength(500)]
        public string? Observacoes { get; set; }
        
        [MaxLength(20)]
        public string Status { get; set; } = "Ativo";
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}
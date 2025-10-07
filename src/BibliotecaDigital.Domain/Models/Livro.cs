using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaDigital.Domain.Models
{
    public class Livro
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;
        
        [Required]
        public int AutorId { get; set; }
        
        [ForeignKey("AutorId")]
        public virtual Autor Autor { get; set; } = null!;
        
        [MaxLength(20)]
        public string? ISBN { get; set; }
        
        public int? AnoPublicacao { get; set; }
        
        [MaxLength(100)]
        public string? Editora { get; set; }
        
        [MaxLength(50)]
        public string? Genero { get; set; }
        
        public int? NumeroEdicao { get; set; }
        
        public int? NumeroPaginas { get; set; }
        
        [MaxLength(20)]
        public string? Idioma { get; set; }
        
        [MaxLength(2000)]
        public string? Sinopse { get; set; }
        
        [MaxLength(500)]
        public string? CapaUrl { get; set; }
        
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Preco { get; set; }
        
        public int EstoqueDisponivel { get; set; }
        
        public int EstoqueTotal { get; set; }
        
        public bool Ativo { get; set; } = true;
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
        
        // Relacionamento 1:N - Um livro pode ter muitos empréstimos
        public virtual ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
    }
}
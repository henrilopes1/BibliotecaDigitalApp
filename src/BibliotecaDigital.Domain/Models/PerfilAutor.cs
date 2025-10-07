using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaDigital.Domain.Models
{
    public class PerfilAutor
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int AutorId { get; set; }
        
        [ForeignKey("AutorId")]
        public virtual Autor Autor { get; set; } = null!;
        
        [MaxLength(2000)]
        public string? Biografia { get; set; }
        
        [MaxLength(500)]
        public string? FotoUrl { get; set; }
        
        [MaxLength(200)]
        public string? Website { get; set; }
        
        [MaxLength(1000)]
        public string? RedesSociais { get; set; }
        
        [MaxLength(1000)]
        public string? Premios { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
    }
}
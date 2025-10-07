using System.ComponentModel.DataAnnotations;

namespace BibliotecaDigital.Domain.Models
{
    public class Autor
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? Email { get; set; }
        
        public DateTime? DataNascimento { get; set; }
        
        [MaxLength(50)]
        public string? Nacionalidade { get; set; }
        
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
        
        // Relacionamento 1:N - Um autor pode ter muitos livros
        public virtual ICollection<Livro> Livros { get; set; } = new List<Livro>();
        
        // Relacionamento 1:1 - Um autor tem um perfil detalhado
        public virtual PerfilAutor? Perfil { get; set; }
    }
}
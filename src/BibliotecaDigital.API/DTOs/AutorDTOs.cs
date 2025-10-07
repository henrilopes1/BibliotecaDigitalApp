using System.ComponentModel.DataAnnotations;

namespace BibliotecaDigital.API.DTOs
{
    // DTOs para Autor
    public class AutorDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Email { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Nacionalidade { get; set; }
        public PerfilAutorDTO? Perfil { get; set; }
        public List<LivroResumoDTO> Livros { get; set; } = new();
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class CreateAutorDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [MaxLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        public DateTime? DataNascimento { get; set; }

        [MaxLength(50, ErrorMessage = "Nacionalidade deve ter no máximo 50 caracteres")]
        public string? Nacionalidade { get; set; }

        public CreatePerfilAutorDTO? Perfil { get; set; }
    }

    public class UpdateAutorDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
        [MaxLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
        public string? Email { get; set; }

        public DateTime? DataNascimento { get; set; }

        [MaxLength(50, ErrorMessage = "Nacionalidade deve ter no máximo 50 caracteres")]
        public string? Nacionalidade { get; set; }
    }

    // DTOs para PerfilAutor
    public class PerfilAutorDTO
    {
        public int Id { get; set; }
        public int AutorId { get; set; }
        public string? Biografia { get; set; }
        public string? FotoUrl { get; set; }
        public string? Website { get; set; }
        public string? RedesSociais { get; set; }
        public string? Premios { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class CreatePerfilAutorDTO
    {
        [MaxLength(2000, ErrorMessage = "Biografia deve ter no máximo 2000 caracteres")]
        public string? Biografia { get; set; }

        [MaxLength(500, ErrorMessage = "URL da foto deve ter no máximo 500 caracteres")]
        [Url(ErrorMessage = "URL da foto deve ter um formato válido")]
        public string? FotoUrl { get; set; }

        [MaxLength(200, ErrorMessage = "Website deve ter no máximo 200 caracteres")]
        [Url(ErrorMessage = "Website deve ter um formato válido")]
        public string? Website { get; set; }

        [MaxLength(1000, ErrorMessage = "Redes sociais deve ter no máximo 1000 caracteres")]
        public string? RedesSociais { get; set; }

        [MaxLength(1000, ErrorMessage = "Prêmios deve ter no máximo 1000 caracteres")]
        public string? Premios { get; set; }
    }

    public class UpdatePerfilAutorDTO
    {
        [MaxLength(2000, ErrorMessage = "Biografia deve ter no máximo 2000 caracteres")]
        public string? Biografia { get; set; }

        [MaxLength(500, ErrorMessage = "URL da foto deve ter no máximo 500 caracteres")]
        [Url(ErrorMessage = "URL da foto deve ter um formato válido")]
        public string? FotoUrl { get; set; }

        [MaxLength(200, ErrorMessage = "Website deve ter no máximo 200 caracteres")]
        [Url(ErrorMessage = "Website deve ter um formato válido")]
        public string? Website { get; set; }

        [MaxLength(1000, ErrorMessage = "Redes sociais deve ter no máximo 1000 caracteres")]
        public string? RedesSociais { get; set; }

        [MaxLength(1000, ErrorMessage = "Prêmios deve ter no máximo 1000 caracteres")]
        public string? Premios { get; set; }
    }
}
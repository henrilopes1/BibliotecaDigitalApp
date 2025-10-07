using System.ComponentModel.DataAnnotations;

namespace BibliotecaDigital.API.DTOs
{
    // DTOs para Livro
    public class LivroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int AutorId { get; set; }
        public AutorResumoDTO Autor { get; set; } = new();
        public string? ISBN { get; set; }
        public int? AnoPublicacao { get; set; }
        public string? Editora { get; set; }
        public string? Genero { get; set; }
        public int? NumeroEdicao { get; set; }
        public int? NumeroPaginas { get; set; }
        public string? Idioma { get; set; }
        public string? Sinopse { get; set; }
        public string? CapaUrl { get; set; }
        public decimal? Preco { get; set; }
        public int EstoqueDisponivel { get; set; }
        public int EstoqueTotal { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }

    public class LivroResumoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int? AnoPublicacao { get; set; }
        public string? Editora { get; set; }
        public string? Genero { get; set; }
        public decimal? Preco { get; set; }
        public int EstoqueDisponivel { get; set; }
        public bool Ativo { get; set; }
    }

    public class CreateLivroDTO
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "AutorId é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "AutorId deve ser um número positivo")]
        public int AutorId { get; set; }

        [MaxLength(20, ErrorMessage = "ISBN deve ter no máximo 20 caracteres")]
        public string? ISBN { get; set; }

        [Range(1, 9999, ErrorMessage = "Ano de publicação deve estar entre 1 e 9999")]
        public int? AnoPublicacao { get; set; }

        [MaxLength(100, ErrorMessage = "Editora deve ter no máximo 100 caracteres")]
        public string? Editora { get; set; }

        [MaxLength(50, ErrorMessage = "Gênero deve ter no máximo 50 caracteres")]
        public string? Genero { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Número da edição deve ser positivo")]
        public int? NumeroEdicao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Número de páginas deve ser positivo")]
        public int? NumeroPaginas { get; set; }

        [MaxLength(20, ErrorMessage = "Idioma deve ter no máximo 20 caracteres")]
        public string? Idioma { get; set; }

        [MaxLength(2000, ErrorMessage = "Sinopse deve ter no máximo 2000 caracteres")]
        public string? Sinopse { get; set; }

        [MaxLength(500, ErrorMessage = "URL da capa deve ter no máximo 500 caracteres")]
        [Url(ErrorMessage = "URL da capa deve ter um formato válido")]
        public string? CapaUrl { get; set; }

        [Range(0, 99999.99, ErrorMessage = "Preço deve estar entre 0 e 99999.99")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "Estoque disponível é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque disponível deve ser maior ou igual a 0")]
        public int EstoqueDisponivel { get; set; }

        [Required(ErrorMessage = "Estoque total é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque total deve ser maior ou igual a 0")]
        public int EstoqueTotal { get; set; }
    }

    public class UpdateLivroDTO
    {
        [Required(ErrorMessage = "Título é obrigatório")]
        [MaxLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "AutorId é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "AutorId deve ser um número positivo")]
        public int AutorId { get; set; }

        [MaxLength(20, ErrorMessage = "ISBN deve ter no máximo 20 caracteres")]
        public string? ISBN { get; set; }

        [Range(1, 9999, ErrorMessage = "Ano de publicação deve estar entre 1 e 9999")]
        public int? AnoPublicacao { get; set; }

        [MaxLength(100, ErrorMessage = "Editora deve ter no máximo 100 caracteres")]
        public string? Editora { get; set; }

        [MaxLength(50, ErrorMessage = "Gênero deve ter no máximo 50 caracteres")]
        public string? Genero { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Número da edição deve ser positivo")]
        public int? NumeroEdicao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Número de páginas deve ser positivo")]
        public int? NumeroPaginas { get; set; }

        [MaxLength(20, ErrorMessage = "Idioma deve ter no máximo 20 caracteres")]
        public string? Idioma { get; set; }

        [MaxLength(2000, ErrorMessage = "Sinopse deve ter no máximo 2000 caracteres")]
        public string? Sinopse { get; set; }

        [MaxLength(500, ErrorMessage = "URL da capa deve ter no máximo 500 caracteres")]
        [Url(ErrorMessage = "URL da capa deve ter um formato válido")]
        public string? CapaUrl { get; set; }

        [Range(0, 99999.99, ErrorMessage = "Preço deve estar entre 0 e 99999.99")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "Estoque disponível é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque disponível deve ser maior ou igual a 0")]
        public int EstoqueDisponivel { get; set; }

        [Required(ErrorMessage = "Estoque total é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque total deve ser maior ou igual a 0")]
        public int EstoqueTotal { get; set; }

        public bool Ativo { get; set; } = true;
    }

    // DTO para Autor resumido (usado no LivroDTO)
    public class AutorResumoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Nacionalidade { get; set; }
    }
}
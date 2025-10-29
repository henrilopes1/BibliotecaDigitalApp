namespace BibliotecaDigital.API.DTOs
{
    // DTOs para Open Library API
    public class OpenLibrarySearchResponseDto
    {
        public int NumFound { get; set; }
        public int Start { get; set; }
        public bool NumFoundExact { get; set; }
        public List<OpenLibraryBookDto> Docs { get; set; } = new();
    }

    public class OpenLibraryBookDto
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string>? Author_Name { get; set; }
        public List<string>? Isbn { get; set; }
        public int? First_Publish_Year { get; set; }
        public List<string>? Subject { get; set; }
        public int? Number_Of_Pages_Median { get; set; }
        public string? Language { get; set; }
        public List<string>? Publisher { get; set; }
        public double? Ratings_Average { get; set; }
        public int? Ratings_Count { get; set; }
        public bool Has_Fulltext { get; set; }
        public List<long>? Cover_I { get; set; }
    }

    // DTOs para resposta enriquecida
    public class LivroEnriquecidoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public bool Disponivel { get; set; }
        
        // Dados do autor interno
        public AutorDTO Autor { get; set; } = new();
        
        // Dados enriquecidos da Open Library
        public DadosEnriquecidosDto DadosExternos { get; set; } = new();
        
        // URLs das capas
        public CapasLivroDto Capas { get; set; } = new();
    }

    public class DadosEnriquecidosDto
    {
        public List<string> AutoresExternos { get; set; } = new();
        public List<string> Categorias { get; set; } = new();
        public List<string> Editoras { get; set; } = new();
        public int? NumeroPaginas { get; set; }
        public double? AvaliacaoMedia { get; set; }
        public int? NumeroAvaliacoes { get; set; }
        public List<string> Idiomas { get; set; } = new();
        public bool TemTextoCompleto { get; set; }
        public List<string> ISBNs { get; set; } = new();
        public string ChaveOpenLibrary { get; set; } = string.Empty;
    }

    public class CapasLivroDto
    {
        public string? CapaPequena { get; set; }
        public string? CapaMedia { get; set; }
        public string? CapaGrande { get; set; }
    }

    // DTO para busca externa
    public class LivroExternoDto
    {
        public string ChaveOpenLibrary { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public List<string> Autores { get; set; } = new();
        public int? AnoPublicacao { get; set; }
        public List<string> Categorias { get; set; } = new();
        public List<string> ISBNs { get; set; } = new();
        public bool ExisteNoBanco { get; set; }
        public int? IdLivroInterno { get; set; }
        public CapasLivroDto Capas { get; set; } = new();
        public double? AvaliacaoMedia { get; set; }
        public int? NumeroAvaliacoes { get; set; }
    }

    // DTO para filtros de busca externa
    public class FiltrosBuscaExternaDto
    {
        public string? Termo { get; set; }
        public string? Autor { get; set; }
        public string? Titulo { get; set; }
        public string? ISBN { get; set; }
        public string? Categoria { get; set; }
        public int? AnoMin { get; set; }
        public int? AnoMax { get; set; }
        public int Limite { get; set; } = 10;
        public bool ApenasComCapas { get; set; } = false;
    }

    // DTO para importar livro externo
    public class ImportarLivroExternoDto
    {
        public string ChaveOpenLibrary { get; set; } = string.Empty;
        public int AutorId { get; set; }
        public string? CategoriaPersonalizada { get; set; }
    }
}
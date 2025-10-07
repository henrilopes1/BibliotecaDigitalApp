using BibliotecaDigital.API.DTOs;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDigital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroRepository _livroRepository;
        private readonly IAutorRepository _autorRepository;

        public LivrosController(ILivroRepository livroRepository, IAutorRepository autorRepository)
        {
            _livroRepository = livroRepository;
            _autorRepository = autorRepository;
        }

        /// <summary>
        /// Obtém todos os livros
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetAll()
        {
            var livros = await _livroRepository.GetAllAsync();
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Obtém um livro por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LivroDTO>> GetById(int id)
        {
            var livro = await _livroRepository.GetByIdAsync(id);
            if (livro == null)
                return NotFound($"Livro com ID {id} não encontrado.");

            return Ok(MapToDTO(livro));
        }

        /// <summary>
        /// Busca livros por título
        /// </summary>
        [HttpGet("buscar/{titulo}")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> SearchByTitle(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return BadRequest("Título para busca não pode ser vazio.");

            var livros = await _livroRepository.SearchByTitleAsync(titulo);
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Obtém livros por autor
        /// </summary>
        [HttpGet("autor/{autorId}")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetByAutor(int autorId)
        {
            var autorExiste = await _autorRepository.ExistsAsync(autorId);
            if (!autorExiste)
                return NotFound($"Autor com ID {autorId} não encontrado.");

            var livros = await _livroRepository.GetByAutorIdAsync(autorId);
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Obtém livros disponíveis (com estoque > 0)
        /// </summary>
        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<LivroDTO>>> GetAvailable()
        {
            var livros = await _livroRepository.GetAvailableAsync();
            var livrosDTO = livros.Select(MapToDTO);
            return Ok(livrosDTO);
        }

        /// <summary>
        /// Cria um novo livro
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LivroDTO>> Create([FromBody] CreateLivroDTO createLivroDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se o autor existe
            var autorExiste = await _autorRepository.ExistsAsync(createLivroDTO.AutorId);
            if (!autorExiste)
                return BadRequest($"Autor com ID {createLivroDTO.AutorId} não encontrado.");

            var livro = new Livro
            {
                Titulo = createLivroDTO.Titulo,
                AutorId = createLivroDTO.AutorId,
                ISBN = createLivroDTO.ISBN,
                AnoPublicacao = createLivroDTO.AnoPublicacao,
                Editora = createLivroDTO.Editora,
                Genero = createLivroDTO.Genero,
                NumeroEdicao = createLivroDTO.NumeroEdicao,
                NumeroPaginas = createLivroDTO.NumeroPaginas,
                Idioma = createLivroDTO.Idioma,
                Sinopse = createLivroDTO.Sinopse,
                CapaUrl = createLivroDTO.CapaUrl,
                Preco = createLivroDTO.Preco,
                EstoqueDisponivel = createLivroDTO.EstoqueDisponivel,
                EstoqueTotal = createLivroDTO.EstoqueTotal,
                Ativo = true
            };

            var novoLivro = await _livroRepository.AddAsync(livro);
            var livroDTO = MapToDTO(novoLivro);

            return CreatedAtAction(nameof(GetById), new { id = novoLivro.Id }, livroDTO);
        }

        /// <summary>
        /// Atualiza um livro existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<LivroDTO>> Update(int id, [FromBody] UpdateLivroDTO updateLivroDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var livroExistente = await _livroRepository.GetByIdAsync(id);
            if (livroExistente == null)
                return NotFound($"Livro com ID {id} não encontrado.");

            // Verificar se o autor existe
            var autorExiste = await _autorRepository.ExistsAsync(updateLivroDTO.AutorId);
            if (!autorExiste)
                return BadRequest($"Autor com ID {updateLivroDTO.AutorId} não encontrado.");

            livroExistente.Titulo = updateLivroDTO.Titulo;
            livroExistente.AutorId = updateLivroDTO.AutorId;
            livroExistente.ISBN = updateLivroDTO.ISBN;
            livroExistente.AnoPublicacao = updateLivroDTO.AnoPublicacao;
            livroExistente.Editora = updateLivroDTO.Editora;
            livroExistente.Genero = updateLivroDTO.Genero;
            livroExistente.NumeroEdicao = updateLivroDTO.NumeroEdicao;
            livroExistente.NumeroPaginas = updateLivroDTO.NumeroPaginas;
            livroExistente.Idioma = updateLivroDTO.Idioma;
            livroExistente.Sinopse = updateLivroDTO.Sinopse;
            livroExistente.CapaUrl = updateLivroDTO.CapaUrl;
            livroExistente.Preco = updateLivroDTO.Preco;
            livroExistente.EstoqueDisponivel = updateLivroDTO.EstoqueDisponivel;
            livroExistente.EstoqueTotal = updateLivroDTO.EstoqueTotal;
            livroExistente.Ativo = updateLivroDTO.Ativo;

            var livroAtualizado = await _livroRepository.UpdateAsync(livroExistente);
            return Ok(MapToDTO(livroAtualizado));
        }

        /// <summary>
        /// Exclui um livro (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var livroExiste = await _livroRepository.ExistsAsync(id);
            if (!livroExiste)
                return NotFound($"Livro com ID {id} não encontrado.");

            var sucesso = await _livroRepository.DeleteAsync(id);
            if (!sucesso)
                return BadRequest("Não foi possível excluir o livro.");

            return NoContent();
        }

        /// <summary>
        /// Atualiza estoque de um livro
        /// </summary>
        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> UpdateEstoque(int id, [FromBody] int novoEstoque)
        {
            if (novoEstoque < 0)
                return BadRequest("Estoque não pode ser negativo.");

            var sucesso = await _livroRepository.UpdateEstoqueAsync(id, novoEstoque);
            if (!sucesso)
                return NotFound($"Livro com ID {id} não encontrado.");

            return NoContent();
        }

        private static LivroDTO MapToDTO(Livro livro)
        {
            return new LivroDTO
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                AutorId = livro.AutorId,
                Autor = livro.Autor != null ? new AutorResumoDTO
                {
                    Id = livro.Autor.Id,
                    Nome = livro.Autor.Nome,
                    Nacionalidade = livro.Autor.Nacionalidade
                } : new AutorResumoDTO(),
                ISBN = livro.ISBN,
                AnoPublicacao = livro.AnoPublicacao,
                Editora = livro.Editora,
                Genero = livro.Genero,
                NumeroEdicao = livro.NumeroEdicao,
                NumeroPaginas = livro.NumeroPaginas,
                Idioma = livro.Idioma,
                Sinopse = livro.Sinopse,
                CapaUrl = livro.CapaUrl,
                Preco = livro.Preco,
                EstoqueDisponivel = livro.EstoqueDisponivel,
                EstoqueTotal = livro.EstoqueTotal,
                Ativo = livro.Ativo,
                DataCriacao = livro.DataCriacao,
                DataAtualizacao = livro.DataAtualizacao
            };
        }
    }
}
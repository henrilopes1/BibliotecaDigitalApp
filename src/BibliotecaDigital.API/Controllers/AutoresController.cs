using BibliotecaDigital.API.DTOs;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDigital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController : ControllerBase
    {
        private readonly IAutorRepository _autorRepository;

        public AutoresController(IAutorRepository autorRepository)
        {
            _autorRepository = autorRepository;
        }

        /// <summary>
        /// Obtém todos os autores
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> GetAll()
        {
            var autores = await _autorRepository.GetAllAsync();
            var autoresDTO = autores.Select(MapToDTO);
            return Ok(autoresDTO);
        }

        /// <summary>
        /// Obtém um autor por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDTO>> GetById(int id)
        {
            var autor = await _autorRepository.GetByIdAsync(id);
            if (autor == null)
                return NotFound($"Autor com ID {id} não encontrado.");

            return Ok(MapToDTO(autor));
        }

        /// <summary>
        /// Obtém um autor com perfil por ID
        /// </summary>
        [HttpGet("{id}/perfil")]
        public async Task<ActionResult<AutorDTO>> GetWithPerfil(int id)
        {
            var autor = await _autorRepository.GetWithPerfilAsync(id);
            if (autor == null)
                return NotFound($"Autor com ID {id} não encontrado.");

            return Ok(MapToDTO(autor));
        }

        /// <summary>
        /// Obtém um autor com seus livros por ID
        /// </summary>
        [HttpGet("{id}/livros")]
        public async Task<ActionResult<AutorDTO>> GetWithLivros(int id)
        {
            var autor = await _autorRepository.GetWithLivrosAsync(id);
            if (autor == null)
                return NotFound($"Autor com ID {id} não encontrado.");

            return Ok(MapToDTO(autor));
        }

        /// <summary>
        /// Busca autores por nome
        /// </summary>
        [HttpGet("buscar/{nome}")]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> SearchByName(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("Nome para busca não pode ser vazio.");

            var autores = await _autorRepository.SearchByNameAsync(nome);
            var autoresDTO = autores.Select(MapToDTO);
            return Ok(autoresDTO);
        }

        /// <summary>
        /// Cria um novo autor
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AutorDTO>> Create([FromBody] CreateAutorDTO createAutorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var autor = new Autor
            {
                Nome = createAutorDTO.Nome,
                Email = createAutorDTO.Email,
                DataNascimento = createAutorDTO.DataNascimento,
                Nacionalidade = createAutorDTO.Nacionalidade
            };

            // Adicionar perfil se fornecido
            if (createAutorDTO.Perfil != null)
            {
                autor.Perfil = new PerfilAutor
                {
                    Biografia = createAutorDTO.Perfil.Biografia,
                    FotoUrl = createAutorDTO.Perfil.FotoUrl,
                    Website = createAutorDTO.Perfil.Website,
                    RedesSociais = createAutorDTO.Perfil.RedesSociais,
                    Premios = createAutorDTO.Perfil.Premios
                };
            }

            var novoAutor = await _autorRepository.AddAsync(autor);
            var autorDTO = MapToDTO(novoAutor);

            return CreatedAtAction(nameof(GetById), new { id = novoAutor.Id }, autorDTO);
        }

        /// <summary>
        /// Atualiza um autor existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<AutorDTO>> Update(int id, [FromBody] UpdateAutorDTO updateAutorDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var autorExistente = await _autorRepository.GetByIdAsync(id);
            if (autorExistente == null)
                return NotFound($"Autor com ID {id} não encontrado.");

            autorExistente.Nome = updateAutorDTO.Nome;
            autorExistente.Email = updateAutorDTO.Email;
            autorExistente.DataNascimento = updateAutorDTO.DataNascimento;
            autorExistente.Nacionalidade = updateAutorDTO.Nacionalidade;

            var autorAtualizado = await _autorRepository.UpdateAsync(autorExistente);
            return Ok(MapToDTO(autorAtualizado));
        }

        /// <summary>
        /// Exclui um autor
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var autorExiste = await _autorRepository.ExistsAsync(id);
            if (!autorExiste)
                return NotFound($"Autor com ID {id} não encontrado.");

            var sucesso = await _autorRepository.DeleteAsync(id);
            if (!sucesso)
                return BadRequest("Não foi possível excluir o autor. Verifique se não há livros associados.");

            return NoContent();
        }

        private static AutorDTO MapToDTO(Autor autor)
        {
            return new AutorDTO
            {
                Id = autor.Id,
                Nome = autor.Nome,
                Email = autor.Email,
                DataNascimento = autor.DataNascimento,
                Nacionalidade = autor.Nacionalidade,
                Perfil = autor.Perfil != null ? new PerfilAutorDTO
                {
                    Id = autor.Perfil.Id,
                    AutorId = autor.Perfil.AutorId,
                    Biografia = autor.Perfil.Biografia,
                    FotoUrl = autor.Perfil.FotoUrl,
                    Website = autor.Perfil.Website,
                    RedesSociais = autor.Perfil.RedesSociais,
                    Premios = autor.Perfil.Premios,
                    DataCriacao = autor.Perfil.DataCriacao,
                    DataAtualizacao = autor.Perfil.DataAtualizacao
                } : null,
                Livros = autor.Livros?.Select(l => new LivroResumoDTO
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    ISBN = l.ISBN,
                    AnoPublicacao = l.AnoPublicacao,
                    Editora = l.Editora,
                    Genero = l.Genero,
                    Preco = l.Preco,
                    EstoqueDisponivel = l.EstoqueDisponivel,
                    Ativo = l.Ativo
                }).ToList() ?? new List<LivroResumoDTO>(),
                DataCriacao = autor.DataCriacao,
                DataAtualizacao = autor.DataAtualizacao
            };
        }
    }
}
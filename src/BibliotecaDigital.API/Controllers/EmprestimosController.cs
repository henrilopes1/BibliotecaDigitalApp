using BibliotecaDigital.API.DTOs;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BibliotecaDigital.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        private readonly ILivroRepository _livroRepository;

        public EmprestimosController(IEmprestimoRepository emprestimoRepository, ILivroRepository livroRepository)
        {
            _emprestimoRepository = emprestimoRepository;
            _livroRepository = livroRepository;
        }

        /// <summary>
        /// Obtém todos os empréstimos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmprestimoDTO>>> GetAll()
        {
            var emprestimos = await _emprestimoRepository.GetAllAsync();
            var emprestimosDTO = emprestimos.Select(MapToDTO);
            return Ok(emprestimosDTO);
        }

        /// <summary>
        /// Obtém um empréstimo por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmprestimoDTO>> GetById(int id)
        {
            var emprestimo = await _emprestimoRepository.GetByIdAsync(id);
            if (emprestimo == null)
                return NotFound($"Empréstimo com ID {id} não encontrado.");

            return Ok(MapToDTO(emprestimo));
        }

        /// <summary>
        /// Obtém empréstimos ativos de um usuário por CPF
        /// </summary>
        [HttpGet("usuario/{cpf}")]
        public async Task<ActionResult<IEnumerable<EmprestimoDTO>>> GetByUsuario(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return BadRequest("CPF não pode ser vazio.");

            var emprestimos = await _emprestimoRepository.GetEmprestimosAtivosByUsuarioAsync(cpf);
            var emprestimosDTO = emprestimos.Select(MapToDTO);
            return Ok(emprestimosDTO);
        }

        /// <summary>
        /// Obtém empréstimos vencidos
        /// </summary>
        [HttpGet("vencidos")]
        public async Task<ActionResult<IEnumerable<EmprestimoDTO>>> GetVencidos()
        {
            var emprestimos = await _emprestimoRepository.GetEmprestimosVencidosAsync();
            var emprestimosDTO = emprestimos.Select(MapToDTO);
            return Ok(emprestimosDTO);
        }

        /// <summary>
        /// Obtém empréstimos de um livro específico
        /// </summary>
        [HttpGet("livro/{livroId}")]
        public async Task<ActionResult<IEnumerable<EmprestimoDTO>>> GetByLivro(int livroId)
        {
            var livroExiste = await _livroRepository.ExistsAsync(livroId);
            if (!livroExiste)
                return NotFound($"Livro com ID {livroId} não encontrado.");

            var emprestimos = await _emprestimoRepository.GetByLivroIdAsync(livroId);
            var emprestimosDTO = emprestimos.Select(MapToDTO);
            return Ok(emprestimosDTO);
        }

        /// <summary>
        /// Cria um novo empréstimo
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmprestimoDTO>> Create([FromBody] CreateEmprestimoDTO createEmprestimoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verificar se o livro existe e está disponível
            var livro = await _livroRepository.GetByIdAsync(createEmprestimoDTO.LivroId);
            if (livro == null)
                return NotFound($"Livro com ID {createEmprestimoDTO.LivroId} não encontrado.");

            if (!livro.Ativo)
                return BadRequest("Livro não está ativo para empréstimo.");

            if (livro.EstoqueDisponivel <= 0)
                return BadRequest("Livro não disponível no estoque.");

            // Verificar se o usuário já tem empréstimos ativos em excesso (máximo 3)
            var emprestimosAtivos = await _emprestimoRepository.GetEmprestimosAtivosByUsuarioAsync(createEmprestimoDTO.CpfUsuario);
            if (emprestimosAtivos.Count() >= 3)
                return BadRequest("Usuário já possui o máximo de 3 empréstimos ativos.");

            var emprestimo = new Emprestimo
            {
                LivroId = createEmprestimoDTO.LivroId,
                NomeUsuario = createEmprestimoDTO.NomeUsuario,
                CpfUsuario = createEmprestimoDTO.CpfUsuario,
                EmailUsuario = createEmprestimoDTO.EmailUsuario,
                TelefoneUsuario = createEmprestimoDTO.TelefoneUsuario,
                Observacoes = createEmprestimoDTO.Observacoes
            };

            var novoEmprestimo = await _emprestimoRepository.AddAsync(emprestimo);
            var emprestimoDTO = MapToDTO(novoEmprestimo);

            return CreatedAtAction(nameof(GetById), new { id = novoEmprestimo.Id }, emprestimoDTO);
        }

        /// <summary>
        /// Atualiza informações de um empréstimo
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<EmprestimoDTO>> Update(int id, [FromBody] UpdateEmprestimoDTO updateEmprestimoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emprestimoExistente = await _emprestimoRepository.GetByIdAsync(id);
            if (emprestimoExistente == null)
                return NotFound($"Empréstimo com ID {id} não encontrado.");

            if (emprestimoExistente.Devolvido)
                return BadRequest("Não é possível atualizar um empréstimo já devolvido.");

            emprestimoExistente.NomeUsuario = updateEmprestimoDTO.NomeUsuario;
            emprestimoExistente.EmailUsuario = updateEmprestimoDTO.EmailUsuario;
            emprestimoExistente.TelefoneUsuario = updateEmprestimoDTO.TelefoneUsuario;
            emprestimoExistente.Observacoes = updateEmprestimoDTO.Observacoes;

            var emprestimoAtualizado = await _emprestimoRepository.UpdateAsync(emprestimoExistente);
            return Ok(MapToDTO(emprestimoAtualizado));
        }

        /// <summary>
        /// Processa a devolução de um livro
        /// </summary>
        [HttpPatch("{id}/devolver")]
        public async Task<ActionResult<EmprestimoDTO>> Devolver(int id, [FromBody] DevolucaoDTO devolucaoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emprestimo = await _emprestimoRepository.GetByIdAsync(id);
            if (emprestimo == null)
                return NotFound($"Empréstimo com ID {id} não encontrado.");

            if (emprestimo.Devolvido)
                return BadRequest("Livro já foi devolvido.");

            var sucesso = await _emprestimoRepository.DevolverLivroAsync(id, devolucaoDTO.DataDevolucao);
            if (!sucesso)
                return BadRequest("Não foi possível processar a devolução.");

            // Atualizar observações se fornecidas
            if (!string.IsNullOrWhiteSpace(devolucaoDTO.Observacoes))
            {
                emprestimo.Observacoes = devolucaoDTO.Observacoes;
                await _emprestimoRepository.UpdateAsync(emprestimo);
            }

            var emprestimoAtualizado = await _emprestimoRepository.GetByIdAsync(id);
            return Ok(MapToDTO(emprestimoAtualizado!));
        }

        /// <summary>
        /// Calcula a multa por atraso de um empréstimo
        /// </summary>
        [HttpGet("{id}/multa")]
        public async Task<ActionResult<decimal>> CalcularMulta(int id)
        {
            var emprestimoExiste = await _emprestimoRepository.ExistsAsync(id);
            if (!emprestimoExiste)
                return NotFound($"Empréstimo com ID {id} não encontrado.");

            var multa = await _emprestimoRepository.CalcularMultaAtrasoAsync(id);
            return Ok(multa);
        }

        /// <summary>
        /// Exclui um empréstimo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var emprestimoExiste = await _emprestimoRepository.ExistsAsync(id);
            if (!emprestimoExiste)
                return NotFound($"Empréstimo com ID {id} não encontrado.");

            var sucesso = await _emprestimoRepository.DeleteAsync(id);
            if (!sucesso)
                return BadRequest("Não foi possível excluir o empréstimo.");

            return NoContent();
        }

        private static EmprestimoDTO MapToDTO(Emprestimo emprestimo)
        {
            return new EmprestimoDTO
            {
                Id = emprestimo.Id,
                LivroId = emprestimo.LivroId,
                Livro = emprestimo.Livro != null ? new LivroResumoDTO
                {
                    Id = emprestimo.Livro.Id,
                    Titulo = emprestimo.Livro.Titulo,
                    ISBN = emprestimo.Livro.ISBN,
                    AnoPublicacao = emprestimo.Livro.AnoPublicacao,
                    Editora = emprestimo.Livro.Editora,
                    Genero = emprestimo.Livro.Genero,
                    Preco = emprestimo.Livro.Preco,
                    EstoqueDisponivel = emprestimo.Livro.EstoqueDisponivel,
                    Ativo = emprestimo.Livro.Ativo
                } : new LivroResumoDTO(),
                NomeUsuario = emprestimo.NomeUsuario,
                CpfUsuario = emprestimo.CpfUsuario,
                EmailUsuario = emprestimo.EmailUsuario,
                TelefoneUsuario = emprestimo.TelefoneUsuario,
                DataEmprestimo = emprestimo.DataEmprestimo,
                DataDevolucaoPrevista = emprestimo.DataDevolucaoPrevista,
                DataDevolucaoReal = emprestimo.DataDevolucaoReal,
                Devolvido = emprestimo.Devolvido,
                MultaAtraso = emprestimo.MultaAtraso,
                Observacoes = emprestimo.Observacoes,
                Status = emprestimo.Status,
                DataCriacao = emprestimo.DataCriacao,
                DataAtualizacao = emprestimo.DataAtualizacao
            };
        }
    }
}
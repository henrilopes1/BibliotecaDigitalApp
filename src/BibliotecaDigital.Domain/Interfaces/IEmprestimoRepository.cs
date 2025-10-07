using BibliotecaDigital.Domain.Models;

namespace BibliotecaDigital.Domain.Interfaces
{
    public interface IEmprestimoRepository
    {
        Task<IEnumerable<Emprestimo>> GetAllAsync();
        Task<Emprestimo?> GetByIdAsync(int id);
        Task<Emprestimo?> GetWithLivroAsync(int id);
        Task<Emprestimo> AddAsync(Emprestimo emprestimo);
        Task<Emprestimo> UpdateAsync(Emprestimo emprestimo);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Emprestimo>> GetEmprestimosAtivosByUsuarioAsync(string cpfUsuario);
        Task<IEnumerable<Emprestimo>> GetEmprestimosVencidosAsync();
        Task<IEnumerable<Emprestimo>> GetByLivroIdAsync(int livroId);
        Task<bool> DevolverLivroAsync(int emprestimoId, DateTime dataDevolucao);
        Task<decimal> CalcularMultaAtrasoAsync(int emprestimoId);
    }
}
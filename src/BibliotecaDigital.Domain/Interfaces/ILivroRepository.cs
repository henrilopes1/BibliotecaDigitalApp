using BibliotecaDigital.Domain.Models;

namespace BibliotecaDigital.Domain.Interfaces
{
    public interface ILivroRepository
    {
        Task<IEnumerable<Livro>> GetAllAsync();
        Task<Livro?> GetByIdAsync(int id);
        Task<Livro?> GetWithAutorAsync(int id);
        Task<Livro> AddAsync(Livro livro);
        Task<Livro> UpdateAsync(Livro livro);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Livro>> SearchByTitleAsync(string titulo);
        Task<IEnumerable<Livro>> GetByAutorIdAsync(int autorId);
        Task<IEnumerable<Livro>> GetAvailableAsync();
        Task<bool> UpdateEstoqueAsync(int livroId, int novoEstoque);
    }
}
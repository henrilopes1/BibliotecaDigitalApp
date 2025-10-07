using BibliotecaDigital.Domain.Models;

namespace BibliotecaDigital.Domain.Interfaces
{
    public interface IAutorRepository
    {
        Task<IEnumerable<Autor>> GetAllAsync();
        Task<Autor?> GetByIdAsync(int id);
        Task<Autor?> GetWithPerfilAsync(int id);
        Task<Autor?> GetWithLivrosAsync(int id);
        Task<Autor> AddAsync(Autor autor);
        Task<Autor> UpdateAsync(Autor autor);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Autor>> SearchByNameAsync(string nome);
    }
}
using BibliotecaDigital.Data.Context;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDigital.Data.Repositories
{
    public class AutorRepository : IAutorRepository
    {
        private readonly BibliotecaDigitalContext _context;

        public AutorRepository(BibliotecaDigitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autor>> GetAllAsync()
        {
            return await _context.Autores
                .Include(a => a.Perfil)
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<Autor?> GetByIdAsync(int id)
        {
            return await _context.Autores
                .Include(a => a.Perfil)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Autor?> GetWithPerfilAsync(int id)
        {
            return await _context.Autores
                .Include(a => a.Perfil)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Autor?> GetWithLivrosAsync(int id)
        {
            return await _context.Autores
                .Include(a => a.Perfil)
                .Include(a => a.Livros)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Autor> AddAsync(Autor autor)
        {
            autor.DataCriacao = DateTime.UtcNow;
            await _context.Autores.AddAsync(autor);
            await _context.SaveChangesAsync();
            return autor;
        }

        public async Task<Autor> UpdateAsync(Autor autor)
        {
            autor.DataAtualizacao = DateTime.UtcNow;
            _context.Autores.Update(autor);
            await _context.SaveChangesAsync();
            return autor;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
                return false;

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Autores.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Autor>> SearchByNameAsync(string nome)
        {
            return await _context.Autores
                .Include(a => a.Perfil)
                .Where(a => a.Nome.Contains(nome))
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }
    }
}
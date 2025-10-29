using BibliotecaDigital.Data.Context;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDigital.Data.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private readonly BibliotecaDigitalContext _context;

        public LivroRepository(BibliotecaDigitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Livro>> GetAllAsync()
        {
            return await _context.Livros
                .Include(l => l.Autor)
                .ThenInclude(a => a.Perfil)
                .OrderBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<Livro?> GetByIdAsync(int id)
        {
            return await _context.Livros
                .Include(l => l.Autor)
                .ThenInclude(a => a.Perfil)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Livro?> GetWithAutorAsync(int id)
        {
            return await _context.Livros
                .Include(l => l.Autor)
                .ThenInclude(a => a.Perfil)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Livro> AddAsync(Livro livro)
        {
            livro.DataCriacao = DateTime.UtcNow;
            await _context.Livros.AddAsync(livro);
            await _context.SaveChangesAsync();
            return livro;
        }

        public async Task<Livro> UpdateAsync(Livro livro)
        {
            livro.DataAtualizacao = DateTime.UtcNow;
            _context.Livros.Update(livro);
            await _context.SaveChangesAsync();
            return livro;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var livro = await _context.Livros.FindAsync(id);
            if (livro == null)
                return false;

            livro.Ativo = false;
            livro.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Livros.AnyAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Livro>> SearchByTitleAsync(string titulo)
        {
            return await _context.Livros
                .Include(l => l.Autor)
                .Where(l => l.Titulo.Contains(titulo))
                .OrderBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> GetByAutorIdAsync(int autorId)
        {
            return await _context.Livros
                .Include(l => l.Autor)
                .Where(l => l.AutorId == autorId)
                .OrderBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Livro>> GetAvailableAsync()
        {
            return await _context.Livros
                .Include(l => l.Autor)
                .Where(l => l.EstoqueDisponivel > 0)
                .OrderBy(l => l.Titulo)
                .ToListAsync();
        }

        public async Task<bool> UpdateEstoqueAsync(int livroId, int novoEstoque)
        {
            var livro = await _context.Livros.FindAsync(livroId);
            if (livro == null)
                return false;

            livro.EstoqueDisponivel = novoEstoque;
            livro.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
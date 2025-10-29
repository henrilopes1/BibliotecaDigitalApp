using BibliotecaDigital.Data.Context;
using BibliotecaDigital.Domain.Interfaces;
using BibliotecaDigital.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaDigital.Data.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly BibliotecaDigitalContext _context;

        public EmprestimoRepository(BibliotecaDigitalContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Emprestimo>> GetAllAsync()
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .ThenInclude(l => l.Autor)
                .OrderByDescending(e => e.DataEmprestimo)
                .ToListAsync();
        }

        public async Task<Emprestimo?> GetByIdAsync(int id)
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .ThenInclude(l => l.Autor)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Emprestimo?> GetWithLivroAsync(int id)
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .ThenInclude(l => l.Autor)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Emprestimo> AddAsync(Emprestimo emprestimo)
        {
            emprestimo.DataCriacao = DateTime.UtcNow;
            emprestimo.DataEmprestimo = DateTime.UtcNow;
            emprestimo.DataDevolucaoPrevista = DateTime.UtcNow.AddDays(14); // 14 dias para devolução
            emprestimo.Status = "Ativo";
            // Devolvido é calculado automaticamente com base em DataDevolucaoReal

            await _context.Emprestimos.AddAsync(emprestimo);

            // Atualizar estoque do livro
            var livro = await _context.Livros.FindAsync(emprestimo.LivroId);
            if (livro != null && livro.EstoqueDisponivel > 0)
            {
                livro.EstoqueDisponivel--;
                livro.DataAtualizacao = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return emprestimo;
        }

        public async Task<Emprestimo> UpdateAsync(Emprestimo emprestimo)
        {
            emprestimo.DataAtualizacao = DateTime.UtcNow;
            _context.Emprestimos.Update(emprestimo);
            await _context.SaveChangesAsync();
            return emprestimo;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(id);
            if (emprestimo == null)
                return false;

            // Se o empréstimo não foi devolvido, liberar o estoque
            if (!emprestimo.Devolvido)
            {
                var livro = await _context.Livros.FindAsync(emprestimo.LivroId);
                if (livro != null)
                {
                    livro.EstoqueDisponivel++;
                    livro.DataAtualizacao = DateTime.UtcNow;
                }
            }

            _context.Emprestimos.Remove(emprestimo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Emprestimos.AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Emprestimo>> GetEmprestimosAtivosByUsuarioAsync(string cpfUsuario)
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .ThenInclude(l => l.Autor)
                .Where(e => e.CpfUsuario == cpfUsuario && e.DataDevolucaoReal == null) // Não devolvido
                .OrderByDescending(e => e.DataEmprestimo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> GetEmprestimosVencidosAsync()
        {
            var hoje = DateTime.UtcNow.Date;
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .ThenInclude(l => l.Autor)
                .Where(e => e.DataDevolucaoReal == null && e.DataDevolucaoPrevista.Date < hoje) // Não devolvido e vencido
                .OrderBy(e => e.DataDevolucaoPrevista)
                .ToListAsync();
        }

        public async Task<IEnumerable<Emprestimo>> GetByLivroIdAsync(int livroId)
        {
            return await _context.Emprestimos
                .Include(e => e.Livro)
                .Where(e => e.LivroId == livroId)
                .OrderByDescending(e => e.DataEmprestimo)
                .ToListAsync();
        }

        public async Task<bool> DevolverLivroAsync(int emprestimoId, DateTime dataDevolucao)
        {
            var emprestimo = await _context.Emprestimos
                .Include(e => e.Livro)
                .FirstOrDefaultAsync(e => e.Id == emprestimoId);

            if (emprestimo == null || emprestimo.Devolvido)
                return false;

            emprestimo.DataDevolucaoReal = dataDevolucao; // Define data = Devolvido fica true automaticamente
            emprestimo.Status = "Devolvido";
            emprestimo.DataAtualizacao = DateTime.UtcNow;

            // Calcular multa se houver atraso
            if (dataDevolucao.Date > emprestimo.DataDevolucaoPrevista.Date)
            {
                var diasAtraso = (dataDevolucao.Date - emprestimo.DataDevolucaoPrevista.Date).Days;
                emprestimo.MultaAtraso = diasAtraso * 2.00m; // R$ 2,00 por dia de atraso
            }

            // Devolver livro ao estoque
            if (emprestimo.Livro != null)
            {
                emprestimo.Livro.EstoqueDisponivel++;
                emprestimo.Livro.DataAtualizacao = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalcularMultaAtrasoAsync(int emprestimoId)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(emprestimoId);
            if (emprestimo == null || emprestimo.Devolvido)
                return 0;

            var hoje = DateTime.UtcNow.Date;
            if (hoje <= emprestimo.DataDevolucaoPrevista.Date)
                return 0;

            var diasAtraso = (hoje - emprestimo.DataDevolucaoPrevista.Date).Days;
            return diasAtraso * 2.00m; // R$ 2,00 por dia de atraso
        }
    }
}
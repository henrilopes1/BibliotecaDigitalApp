using BibliotecaDigital.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BibliotecaDigital.Data.Database
{
    public class BibliotecaDigitalContextFactory : IDesignTimeDbContextFactory<BibliotecaDigitalContext>
    {
        public BibliotecaDigitalContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BibliotecaDigitalContext>();
            
            // Para migrations, usar Oracle - você deve substituir pelas suas credenciais reais
            var connectionString = "Data Source=oracle.fiap.com.br:1521/ORCL;User Id=RM98347;Password=290605;";
            optionsBuilder.UseOracle(connectionString);

            return new BibliotecaDigitalContext(optionsBuilder.Options);
        }
    }
}
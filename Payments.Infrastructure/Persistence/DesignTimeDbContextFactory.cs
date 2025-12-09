using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Payments.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PaymentsDbContext>
    {
        public PaymentsDbContext CreateDbContext(string[] args)
        {
            // Detectar el folder de la solución
            var solutionDir = Directory.GetCurrentDirectory();

            // Buscar el proyecto WebApi de manera confiable
            var webApiPath = Path.GetFullPath(Path.Combine(solutionDir, "../Payments.WebApi"));

            var config = new ConfigurationBuilder()
                .SetBasePath(webApiPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false)
                .Build();

            // Obtener cadena de conexión
            var connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new Exception("Error in 'DefaultConnection' in appsettings.json");

            var builder = new DbContextOptionsBuilder<PaymentsDbContext>();
            builder.UseSqlServer(connectionString);

            return new PaymentsDbContext(builder.Options);
        }
    }
}

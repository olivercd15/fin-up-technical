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
            // Ubicar el directorio del proyecto WebApi
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Payments.WebApi");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<PaymentsDbContext>();
            builder.UseSqlServer(connectionString);

            return new PaymentsDbContext(builder.Options);
        }
    }
}

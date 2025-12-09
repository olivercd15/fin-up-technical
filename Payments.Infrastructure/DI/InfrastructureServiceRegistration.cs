using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Interfaces;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Repository;

namespace Payments.Infrastructure.DI
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            // DbContext
            services.AddDbContext<PaymentsDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Dapper Connection
            services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

            // Write side
            services.AddScoped(typeof(IWriteRepository<>), typeof(EfWriteRepository<>));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            // Read side
            services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();

            return services;
        }
    }
}

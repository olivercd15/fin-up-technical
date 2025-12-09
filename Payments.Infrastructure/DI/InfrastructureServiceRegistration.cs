using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Interfaces;
using Payments.Infrastructure.Cache;
using Payments.Infrastructure.Messaging;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Repository;
using Confluent.Kafka;
using StackExchange.Redis;

namespace Payments.Infrastructure.DI
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Missing SQL Server connection string");

            // ============================================================
            // SQL Server (EF Core)
            // ============================================================
            services.AddDbContext<PaymentsDbContext>(options =>
                options.UseSqlServer(connectionString));

            // ============================================================
            // Dapper Connection
            // ============================================================
            services.AddScoped<IDbConnection>(_ => new SqlConnection(connectionString));

            // ============================================================
            // Write side (EF Core)
            // ============================================================
            services.AddScoped(typeof(IWriteRepository<>), typeof(EfWriteRepository<>));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            // ============================================================
            // Read side (Dapper)
            // ============================================================
            services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();

            // ============================================================
            // Redis (Cache)
            // ============================================================
            var redisConn = config.GetConnectionString("Redis");
            if (!string.IsNullOrWhiteSpace(redisConn))
            {
                services.AddSingleton<IConnectionMultiplexer>(_ =>
                    ConnectionMultiplexer.Connect(redisConn));

                services.AddScoped<ICacheService, RedisCacheService>();
            }

            // ============================================================
            // Kafka (Event Bus)
            // ============================================================
            var kafkaServer = config["Kafka:BootstrapServers"];

            if (!string.IsNullOrWhiteSpace(kafkaServer))
            {
                var producerConfig = new ProducerConfig
                {
                    BootstrapServers = kafkaServer
                };

                services.AddSingleton<IProducer<string, string>>(_ =>
                    new ProducerBuilder<string, string>(producerConfig).Build()
                );

                services.AddScoped<IEventBus, KafkaEventBus>();
            }
            else
            {
                services.AddScoped<IEventBus, NullEventBus>();
            }

            return services;
        }
    }
}

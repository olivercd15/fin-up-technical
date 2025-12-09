using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Payments.Application.Payments.DTOs;
using Payments.Application.Interfaces;
using Dapper;

namespace Payments.Infrastructure.Repository
{
    public class PaymentReadRepository : IPaymentReadRepository
    {
        private readonly IDbConnection _connection;

        public PaymentReadRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PaymentListDto>> GetPaymentsByCustomerAsync(Guid customerId)
        {
            var sql = @"
            SELECT 
                payment_id AS PaymentId,
                service_provider AS ServiceProvider,
                amount,
                status,
                created_at AS CreatedAt
            FROM payments
            WHERE customer_id = @CustomerId
            ORDER BY created_at DESC;
        ";

            return await _connection.QueryAsync<PaymentListDto>(sql, new { CustomerId = customerId });
        }
    }
}

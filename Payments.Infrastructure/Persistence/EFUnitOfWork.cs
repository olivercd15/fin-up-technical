using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Application.Interfaces;

namespace Payments.Infrastructure.Persistence
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly PaymentsDbContext _context;

        public EfUnitOfWork(PaymentsDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _context.SaveChangesAsync(ct);
    }
}

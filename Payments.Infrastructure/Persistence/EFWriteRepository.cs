using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Application.Interfaces;

namespace Payments.Infrastructure.Persistence
{
    public class EfWriteRepository<T> : IWriteRepository<T> where T : class
    {
        private readonly PaymentsDbContext _context;

        public EfWriteRepository(PaymentsDbContext context)
        {
            _context = context;
        }

        public void Add(T entity) => _context.Set<T>().Add(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Remove(T entity) => _context.Set<T>().Remove(entity);
    }
}

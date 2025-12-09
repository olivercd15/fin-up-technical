using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces
{
    public interface IReadRepository
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
    }
}

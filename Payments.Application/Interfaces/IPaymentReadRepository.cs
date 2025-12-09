using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Application.Payments.DTOs;

namespace Payments.Application.Interfaces
{
    public interface IPaymentReadRepository
    {
        Task<IEnumerable<PaymentListDto>> GetPaymentsByCustomerAsync(Guid customerId);
    }
}

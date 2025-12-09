using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.Payments.DTOs
{
    public record CreatePaymentDto(
        Guid CustomerId,
        string ServiceProvider,
        decimal Amount,
        string? Currency = "BOB"
    );
}

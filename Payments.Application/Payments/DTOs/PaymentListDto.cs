using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Application.Payments.DTOs
{
    public record PaymentListDto(
        Guid PaymentId,
        string ServiceProvider,
        decimal Amount,
        string Status,
        DateTime CreatedAt
    );
}

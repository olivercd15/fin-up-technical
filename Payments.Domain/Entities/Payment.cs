using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Payments.Domain.Enums;

namespace Payments.Domain.Entities
{
    public sealed class Payment
    {
        public Guid PaymentId { get; private set; } = Guid.NewGuid();
        public Guid CustomerId { get; private set; }
        public string ServiceProvider { get; private set; } = null!;
        public decimal Amount { get; private set; }
        public PaymentCurrency Currency { get; private set; } = PaymentCurrency.BOB;
        public PaymentStatus Status { get; private set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Payment() { } // for EF

        public Payment(Guid customerId, string serviceProvider, decimal amount, PaymentCurrency currency = PaymentCurrency.BOB)
        {
            CustomerId = customerId;
            ServiceProvider = serviceProvider;
            Amount = amount;
            Currency = currency;
        }

        public void MarkRejected() => Status = PaymentStatus.Rejected;
        public void MarkPending() => Status = PaymentStatus.Pending;
        public void MarkCompleted() => Status = PaymentStatus.Completed;
    }
}

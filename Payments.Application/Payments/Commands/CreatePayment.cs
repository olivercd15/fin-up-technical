using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Payments.Application.Common;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;
using Payments.Domain.Enums;

namespace Payments.Application.Payments.Commands
{
    public record CreatePaymentCommand(
        Guid CustomerId,
        string ServiceProvider,
        decimal Amount,
        string Currency = "BOB"
    ) : IRequest<Payment>;


    // =========================
    //       VALIDATOR
    // =========================
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();

            RuleFor(x => x.ServiceProvider).NotEmpty();

            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }


    // =========================
    //        HANDLER
    // =========================
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Payment>
    {
        private readonly IWriteRepository<Payment> _writeRepo;
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreatePaymentCommand> _validator;

        public CreatePaymentCommandHandler(
            IWriteRepository<Payment> writeRepo,
            IUnitOfWork uow,
            IValidator<CreatePaymentCommand> validator)
        {
            _writeRepo = writeRepo;
            _uow = uow;
            _validator = validator;
        }

        public async Task<Payment> Handle(CreatePaymentCommand req, CancellationToken ct)
        {
            // FluentValidation
            var validation = await _validator.ValidateAsync(req, ct);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
                throw new AppValidationException(errors);
            }

            // Business rules
            if (req.Currency == "USD")
                throw new AppException("USD currency is not accepted.");

            if (req.Amount > 1500)
                throw new AppException("Amount exceeds maximum limit (1500 BOB).");

            // Create payment
            var payment = new Payment(
                req.CustomerId,
                req.ServiceProvider,
                req.Amount,
                req.Currency
            );

            _writeRepo.Add(payment);
            await _uow.SaveChangesAsync(ct);

            return payment;
        }
    }
}

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
    ) : IRequest<ApiResponse<Payment>>;


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
    //        EVENT DTO
    // =========================
    public record PaymentCreatedEvent(
        Guid PaymentId,
        Guid CustomerId,
        decimal Amount,
        string ServiceProvider,
        string Currency,
        DateTime CreatedAt
    );


    // =========================
    //        HANDLER
    // =========================
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, ApiResponse<Payment>>
    {
        private readonly IWriteRepository<Payment> _writeRepo;
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreatePaymentCommand> _validator;
        private readonly IEventBus _eventBus;

        public CreatePaymentCommandHandler(
            IWriteRepository<Payment> writeRepo,
            IUnitOfWork uow,
            IValidator<CreatePaymentCommand> validator,
            IEventBus eventBus)
        {
            _writeRepo = writeRepo;
            _uow = uow;
            _validator = validator;
            _eventBus = eventBus;
        }

        public async Task<ApiResponse<Payment>> Handle(CreatePaymentCommand req, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(req, ct);
            if (!validation.IsValid)
            {
                return ApiResponse<Payment>.Fail(400, "Invalid data provided.");
            }

            if (req.Currency == "USD")
                return ApiResponse<Payment>.Fail(400, "USD currency is not accepted.");

            if (req.Amount > 1500)
                return ApiResponse<Payment>.Fail(400, "Amount exceeds allowed limit.");

            var payment = new Payment(
                req.CustomerId,
                req.ServiceProvider,
                req.Amount,
                req.Currency
            );

            _writeRepo.Add(payment);
            await _uow.SaveChangesAsync(ct);

            // Kafka event payment
            //var paymentEvent = new PaymentCreatedEvent(
            //    PaymentId: payment.PaymentId,
            //    CustomerId: payment.CustomerId,
            //    Amount: payment.Amount,
            //    ServiceProvider: payment.ServiceProvider,
            //    Currency: payment.Currency,
            //    CreatedAt: payment.CreatedAt
            //);

            await _eventBus.PublishAsync("payments.created", paymentEvent);

            return ApiResponse<Payment>.Created(payment, "Payment created successfully.");
        }
    }
}

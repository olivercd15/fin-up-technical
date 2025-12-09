using FluentValidation;
using FluentValidation.Results;
using Moq;
using Payments.Application.Common;
using Payments.Application.Interfaces;
using Payments.Application.Payments.Commands;
using Payments.Domain.Entities;
using Xunit;

namespace Payments.Tests.Payments
{
    public class CreatePaymentTests
    {
        private readonly Mock<IWriteRepository<Payment>> _writeRepo = new();
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IValidator<CreatePaymentCommand>> _validator = new();
        private readonly CreatePaymentCommandHandler _handler;

        public CreatePaymentTests()
        {
            _validator
                .Setup(v => v.ValidateAsync(It.IsAny<CreatePaymentCommand>(), default))
                .ReturnsAsync(new ValidationResult());

            _handler = new CreatePaymentCommandHandler(
                _writeRepo.Object,
                _uow.Object,
                _validator.Object
            );
        }

        // ----------------------------------
        // INVALID: Validator fails
        // ----------------------------------
        [Fact]
        public async Task Handle_ShouldReturn400_WhenValidationFails()
        {
            var failures = new List<ValidationFailure>
            {
                new("Amount", "Invalid amount")
            };

            _validator
                .Setup(v => v.ValidateAsync(It.IsAny<CreatePaymentCommand>(), default))
                .ReturnsAsync(new ValidationResult(failures));

            var command = new CreatePaymentCommand(Guid.NewGuid(), "Test", 100);

            var result = await _handler.Handle(command, default);

            Assert.False(result.Success);
            Assert.Equal(400, result.Code);
            Assert.Equal("Invalid data provided.", result.Message);
        }

        // ----------------------------------
        // INVALID: USD Currency
        // ----------------------------------
        [Fact]
        public async Task Handle_ShouldReturn400_WhenCurrencyIsUSD()
        {
            var command = new CreatePaymentCommand(Guid.NewGuid(), "Test", 100, "USD");

            var result = await _handler.Handle(command, default);

            Assert.False(result.Success);
            Assert.Equal(400, result.Code);
            Assert.Equal("USD currency is not accepted.", result.Message);
        }

        // ----------------------------------
        // INVALID: Amount > 1500
        // ----------------------------------
        [Fact]
        public async Task Handle_ShouldReturn400_WhenAmountExceedsLimit()
        {
            var command = new CreatePaymentCommand(Guid.NewGuid(), "Test", 2000);

            var result = await _handler.Handle(command, default);

            Assert.False(result.Success);
            Assert.Equal(400, result.Code);
            Assert.Equal("Amount exceeds allowed limit.", result.Message);
        }

        // ----------------------------------
        // SUCCESS
        // ----------------------------------
        [Fact]
        public async Task Handle_ShouldCreatePayment_WhenDataIsValid()
        {
            var command = new CreatePaymentCommand(Guid.NewGuid(), "Service", 100, "BOB");

            var result = await _handler.Handle(command, default);

            Assert.True(result.Success);
            Assert.Equal(201, result.Code);
            Assert.Equal("Payment created successfully.", result.Message);
            Assert.NotNull(result.Data);

            _writeRepo.Verify(r => r.Add(It.IsAny<Payment>()), Times.Once);
            _uow.Verify(u => u.SaveChangesAsync(default), Times.Once);
        }
    }
}

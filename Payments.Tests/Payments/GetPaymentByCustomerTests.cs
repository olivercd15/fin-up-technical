using Xunit;
using Moq;
using Payments.Application.Payments.Queries;
using Payments.Application.Interfaces;
using Payments.Application.Payments.DTOs;
using Payments.Application.Common;

namespace Payments.Tests.Payments
{
    public class GetPaymentsByCustomerHandlerTests
    {
        private readonly Mock<IPaymentReadRepository> _readRepo = new();

        [Fact]
        public async Task Handle_ShouldReturn404_WhenNoPaymentsFound()
        {
            _readRepo
                .Setup(r => r.GetPaymentsByCustomerAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<PaymentListDto>());

            var handler = new GetPaymentsByCustomerHandler(_readRepo.Object);

            var result = await handler.Handle(
                new GetPaymentsByCustomerQuery(Guid.NewGuid()), default);

            Assert.False(result.Success);
            Assert.Equal(404, result.Code);
            Assert.Equal("No payments found for this customer.", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnPayments_WhenPaymentsExist()
        {
            var mockList = new List<PaymentListDto>
            {
                new PaymentListDto(
                    Guid.NewGuid(),
                    "SERVICIO DE ELECTRICIDAD",
                    100,
                    "Pending",
                    DateTime.UtcNow
                )
            };

            _readRepo
                .Setup(r => r.GetPaymentsByCustomerAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockList);

            var handler = new GetPaymentsByCustomerHandler(_readRepo.Object);

            var result = await handler.Handle(
                new GetPaymentsByCustomerQuery(Guid.NewGuid()), default);

            Assert.True(result.Success);
            Assert.Equal(200, result.Code);
            Assert.Equal("Payments retrieved successfully.", result.Message);
            Assert.Single(result.Data);
        }
    }
}

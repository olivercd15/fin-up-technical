using MediatR;
using Payments.Application.Interfaces;
using Payments.Application.Payments.DTOs;

namespace Payments.Application.Payments.Queries
{
    public record GetPaymentsByCustomerQuery(Guid CustomerId)
        : IRequest<IEnumerable<PaymentListDto>>;

    // =========================
    //        HANDLER
    // =========================
    public class GetPaymentsByCustomerHandler
        : IRequestHandler<GetPaymentsByCustomerQuery, IEnumerable<PaymentListDto>>
    {
        private readonly IPaymentReadRepository _readRepo;

        public GetPaymentsByCustomerHandler(IPaymentReadRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<IEnumerable<PaymentListDto>> Handle(
            GetPaymentsByCustomerQuery req,
            CancellationToken ct)
        {
            return await _readRepo.GetPaymentsByCustomerAsync(req.CustomerId);
        }
    }
}

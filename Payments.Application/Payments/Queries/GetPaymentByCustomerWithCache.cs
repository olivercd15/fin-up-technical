using MediatR;
using Payments.Application.Common;
using Payments.Application.Interfaces;
using Payments.Application.Payments.DTOs;

namespace Payments.Application.Payments.Queries
{
    public record GetPaymentsByCustomerWithCacheQuery(Guid CustomerId) : IRequest<ApiResponse<IEnumerable<PaymentListDto>>>;

    // =========================
    //        HANDLER
    // =========================
    public class GetPaymentsByCustomerWithCacheHandler
        : IRequestHandler<GetPaymentsByCustomerWithCacheQuery, ApiResponse<IEnumerable<PaymentListDto>>>
    {
        private readonly IPaymentReadRepository _readRepo;

        public GetPaymentsByCustomerWithCacheHandler(IPaymentReadRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<ApiResponse<IEnumerable<PaymentListDto>>> Handle(GetPaymentsByCustomerWithCacheQuery req, CancellationToken ct)
        {
            var data = await _readRepo.GetPaymentsByCustomerAsync(req.CustomerId);

            if (data is null || !data.Any())
                return ApiResponse<IEnumerable<PaymentListDto>>.Fail(404, "No payments found for this customer.");

            return ApiResponse<IEnumerable<PaymentListDto>>.Ok(data, "Payments retrieved successfully.");
        }
    }
}

using MediatR;
using Payments.Application.Common;
using Payments.Application.Interfaces;
using Payments.Application.Payments.DTOs;

namespace Payments.Application.Payments.Queries
{
    public record GetPaymentsByCustomerQuery(Guid CustomerId) : IRequest<ApiResponse<IEnumerable<PaymentListDto>>>;

    // =========================
    //        HANDLER
    // =========================
    public class GetPaymentsByCustomerHandler
        : IRequestHandler<GetPaymentsByCustomerQuery, ApiResponse<IEnumerable<PaymentListDto>>>
    {
        private readonly IPaymentReadRepository _readRepo;

        public GetPaymentsByCustomerHandler(IPaymentReadRepository readRepo)
        {
            _readRepo = readRepo;
        }

        public async Task<ApiResponse<IEnumerable<PaymentListDto>>> Handle(GetPaymentsByCustomerQuery req, CancellationToken ct)
        {
            var data = await _readRepo.GetPaymentsByCustomerAsync(req.CustomerId);

            if (data is null || !data.Any())
                return ApiResponse<IEnumerable<PaymentListDto>>.Fail(404, "No payments found for this customer.");

            return ApiResponse<IEnumerable<PaymentListDto>>.Ok(data, "Payments retrieved successfully.");
        }
    }
}

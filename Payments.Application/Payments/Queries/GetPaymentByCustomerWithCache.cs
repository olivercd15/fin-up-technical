using MediatR;
using Payments.Application.Common;
using Payments.Application.Interfaces;
using Payments.Application.Payments.DTOs;

namespace Payments.Application.Payments.Queries
{
    public record GetPaymentsByCustomerWithCacheQuery(Guid CustomerId)
        : IRequest<ApiResponse<IEnumerable<PaymentListDto>>>;

    public class GetPaymentsByCustomerWithCacheHandler
        : IRequestHandler<GetPaymentsByCustomerWithCacheQuery, ApiResponse<IEnumerable<PaymentListDto>>>
    {
        private readonly IPaymentReadRepository _readRepo;
        private readonly ICacheService _cache;

        public GetPaymentsByCustomerWithCacheHandler(
            IPaymentReadRepository readRepo,
            ICacheService cache)
        {
            _readRepo = readRepo;
            _cache = cache;
        }

        public async Task<ApiResponse<IEnumerable<PaymentListDto>>> Handle(
            GetPaymentsByCustomerWithCacheQuery req,
            CancellationToken ct)
        {
            string cacheKey = $"payments:{req.CustomerId}";

            var cached = await _cache.GetAsync<IEnumerable<PaymentListDto>>(cacheKey);
            if (cached is not null)
            {
                return ApiResponse<IEnumerable<PaymentListDto>>.Ok(
                    cached, "Payments retrieved from cache.");
            }

            // Go to database
            var data = await _readRepo.GetPaymentsByCustomerAsync(req.CustomerId);

            if (data is null || !data.Any())
                return ApiResponse<IEnumerable<PaymentListDto>>.Fail(404, "No payments found for this customer.");

            // Store in cache
            await _cache.SetAsync(cacheKey, data, TimeSpan.FromSeconds(120));

            return ApiResponse<IEnumerable<PaymentListDto>>.Ok(data, "Payments retrieved successfully.");
        }
    }
}

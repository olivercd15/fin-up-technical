using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Payments.Commands;
using Payments.Application.Payments.Queries;

namespace Payments.WebApi.Controllers.Payments
{
    [ApiController]
    [Route("api/v2/Payments")]
    public class PaymentsV2Controller : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsV2Controller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWithMessaging(CreatePaymentWithMessagingCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCustomerWithCache(Guid customerId)
        {
            var result = await _mediator.Send(new GetPaymentsByCustomerQuery(customerId));
            return StatusCode(result.Code, result);
        }
    }
}

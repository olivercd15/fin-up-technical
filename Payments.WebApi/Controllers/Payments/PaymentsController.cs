using MediatR;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Payments.Commands;
using Payments.Application.Payments.Queries;

namespace Payments.WebApi.Controllers.Payments
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.Code, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCustomer(Guid customerId)
        {
            var result = await _mediator.Send(new GetPaymentsByCustomerQuery(customerId));
            return StatusCode(result.Code, result);
        }
    }
}

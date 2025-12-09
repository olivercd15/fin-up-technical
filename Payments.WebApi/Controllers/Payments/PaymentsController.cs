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

        // ======================================
        // POST api/payments
        // ======================================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentCommand command)
        {
            var payment = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetByCustomer),
                new { customerId = payment.CustomerId },
                payment
            );
        }

        // ======================================
        // GET api/payments?customerId=...
        // ======================================
        [HttpGet]
        public async Task<IActionResult> GetByCustomer([FromQuery] Guid customerId)
        {
            var result = await _mediator.Send(new GetPaymentsByCustomerQuery(customerId));

            return Ok(result);
        }
    }
}

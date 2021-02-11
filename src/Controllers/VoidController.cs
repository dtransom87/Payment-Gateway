using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using payment_gateway.Models;
using payment_gateway.Services;

namespace payment_gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class VoidController: ControllerBase
    {
        private readonly ILogger<VoidController> _logger;
        private readonly IPaymentService _paymentService;

        public VoidController(ILogger<VoidController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> Void([FromQuery] string paymentId)
        {
            var payment = await _paymentService.GetPaymentById(paymentId);
            if (payment == default)
                return NotFound();
   
            await _paymentService.VoidPayment(payment);    

            return Accepted(payment);
        }
    }
}

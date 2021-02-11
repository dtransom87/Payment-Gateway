using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using payment_gateway.Models;
using payment_gateway.Models.Requests;
using payment_gateway.Services;

namespace payment_gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]        
    public class RefundController : ControllerBase
    {
        private readonly ILogger<RefundController> _logger;
        private readonly IPaymentService _paymentService;

        public RefundController(ILogger<RefundController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> Refund([FromBody] UpdatePaymentRequest request)
        {
            try
            {
                var payment = await _paymentService.GetPaymentById(request.PaymentId);
                if (payment == default)
                    return NotFound("No payment found with that ID");
   
                await _paymentService.RefundPayment(payment, request.TransactionDetails);    

                return Accepted(payment);
            }
            catch(PaymentGatewayServiceException pgse)
            {
                _logger.LogError("Caugh bad known error from repo");
                return UnprocessableEntity(pgse.Message);
            }
              
            catch(InvalidTransactionCaptureException intce)
            {
                _logger.LogError($"Problem refunding payment - {intce.Message}");
                return BadRequest(intce.Message);
            }                
        }
    }
}

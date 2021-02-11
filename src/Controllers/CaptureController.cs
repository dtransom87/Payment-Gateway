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
    public class CaptureController : ControllerBase
    {
        private readonly ILogger<CaptureController> _logger;
        private readonly IPaymentService _paymentService;

        public CaptureController(ILogger<CaptureController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> Capture([FromBody] UpdatePaymentRequest request)
        {
            try
            {
                var payment = await _paymentService.GetPaymentById(request.PaymentId);
                if (payment == default)
                    return NotFound();
   
                await _paymentService.CapturePayment(payment, request.TransactionDetails);    

                return Accepted(payment);
            }
            catch(PaymentGatewayServiceException pgse)
            {
                _logger.LogError("Caught bad known error from repo");
                return UnprocessableEntity(pgse.Message);
            }  
            catch(InvalidTransactionCaptureException intce)
            {
                _logger.LogError($"Problem capturing payment - {intce.Message}");
                return BadRequest(intce.Message);
            }    
        }
    }
}

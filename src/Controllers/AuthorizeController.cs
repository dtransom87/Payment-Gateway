using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using payment_gateway.Models;
using payment_gateway.Models.Requests;
using payment_gateway.Services;

namespace payment_gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizeController : ControllerBase
    {
        private readonly ILogger<AuthorizeController> _logger;
        private readonly IValidator<AuthorisationRequest> _validator;
        private readonly IPaymentService _paymentService;

        public AuthorizeController(ILogger<AuthorizeController> logger, IValidator<AuthorisationRequest> validator, IPaymentService paymentService)
        {
            _logger = logger;
            _validator = validator;
            _paymentService = paymentService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> Create([FromBody] AuthorisationRequest createRequest)
        {
            try
            {
                var errors = await ValidateRequest(createRequest);
                if (errors != null)
                {
                    return BadRequest(errors);
                }

                var payment = new Payment
                {
                    CreditCardDetails = createRequest.CreditCardData,
                    TransactionDetails = createRequest.TransactionDetails,
                };

                await _paymentService.CreatePayment(payment);

                return CreatedAtAction(nameof(GetByPaymentId), new {paymentId = payment.Id}, payment);
            }
            catch(PaymentGatewayServiceException pgse)
            {
                _logger.LogError("Caugh bad known error from repo");
                return UnprocessableEntity(pgse.Message);
            }            
        }

        [HttpGet("{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> GetByPaymentId([FromRoute] string paymentId)
        {
            return await _paymentService.GetPaymentById(paymentId);
        }

        private async Task<string> ValidateRequest(AuthorisationRequest request)
        {
            var result = await _validator.ValidateAsync(request);
            if (result.IsValid) return null;
            var validationErrors = JsonConvert.SerializeObject(result.Errors.Select(e => e.ErrorMessage));
            _logger.LogDebug($"Validation Errors : {validationErrors}");
            return validationErrors;

        }
    }
}

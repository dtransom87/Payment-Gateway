using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using payment_gateway.Models;
using payment_gateway.Repositories;

namespace payment_gateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _repository;

        private readonly ILogger<PaymentService> _logger;
        public PaymentService(IRepository<Payment> repository, ILogger<PaymentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }    

        public async Task<Payment> CreatePayment(Payment payment)
        {
            if (payment.CreditCardDetails.CreditCardNumber == 4000000000000119)
                throw new PaymentGatewayServiceException("Invalid credit card number for Authorize");            
            return await _repository.Create(payment);
        }

        public async Task<Payment> GetPaymentById(string id)
        {
            var payment = await _repository.Get(id);
            if (payment == default)
                throw new Exception("Can't find payment");

            return payment;    
        }

        public async Task<Payment> VoidPayment(Payment payment)
        {
            payment.IsVoid = true;
            return await _repository.Update(payment);
        }

        public async Task<Payment> CapturePayment(Payment payment, TransactionUpdateDetails details)
        {
            if (payment.CreditCardDetails.CreditCardNumber == 4000000000000259)
                throw new PaymentGatewayServiceException("Invalid credit card number for Capture");

            // Would these checks into a rule engine but for now hard coding due to time constaints
            if (payment.IsVoid)
            {
                throw new InvalidTransactionCaptureException("Payment is already void so should not capture", payment.Id);
            };

            if (payment.TransactionRefundDetails != default)
            {
                throw new InvalidTransactionCaptureException("Payment has been refunded", payment.Id);
            }

            if (details.Amount < 0)
            {
                throw new InvalidTransactionCaptureException("Amount can not be less than 0", payment.Id);
            }
          
            if (payment.PaymentCompletedDate != DateTime.MinValue)
            {
                throw new InvalidTransactionCaptureException("Payment has been fully paid", payment.Id);
            }

            if(payment.TransactionCaptureDetails == default)
            {
              payment.TransactionCaptureDetails = new List<TransactionUpdateDetails>
              {
                  details
              };
            }
            else
            {
                payment.TransactionCaptureDetails.Add(details);
            }   

            var paidAmount = payment.TransactionCaptureDetails.Sum(x => x.Amount);

            if (payment.TransactionDetails.Amount - paidAmount < 0)
            {
                throw new InvalidTransactionCaptureException("Transaction will take total amount greater than what customer owes", payment.Id);
            }  

            if (payment.TransactionDetails.Amount == paidAmount)
            {
                _logger.LogInformation("Payment amount owed is now 0. Do not capture any more money");
                payment.PaymentCompletedDate = DateTime.UtcNow;
            }

            return await _repository.Update(payment);
        }

        public async Task<Payment> RefundPayment(Payment payment, TransactionUpdateDetails details)
        {
            if (payment.CreditCardDetails.CreditCardNumber == 4000000000003238)
                throw new PaymentGatewayServiceException("Invalid credit card number for Refund");

            if(payment.TransactionRefundDetails == default)
            {
              payment.TransactionRefundDetails = new List<TransactionUpdateDetails>
              {
                  details
              };
            }
            else
            {
                payment.TransactionRefundDetails.Add(details);
            }                   

            var refundAmount = payment.TransactionRefundDetails.Sum(x => x.Amount);
            decimal totalAmountCaptured = 0m;
            if (payment.TransactionCaptureDetails != default)
            {
                totalAmountCaptured = payment.TransactionCaptureDetails.Sum(x => x.Amount);
            }

            if (totalAmountCaptured - refundAmount < 0)
            {
                throw new InvalidTransactionCaptureException("This refund will give the customer back more than they paid", payment.Id);
            }  

            if (totalAmountCaptured == refundAmount)
            {
                _logger.LogInformation($"Full payment has been refunded {payment.Id}");
                payment.PaymentCompletedDate = DateTime.UtcNow;
            }

            return await _repository.Update(payment);
        }
    }
}
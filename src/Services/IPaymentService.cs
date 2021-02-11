using System;
using System.Threading.Tasks;
using payment_gateway.Models;

namespace payment_gateway.Services
{
    public interface IPaymentService
    {
        Task<Payment> CreatePayment(Payment payment);
        Task<Payment> GetPaymentById(string id);
        Task<Payment> VoidPayment(Payment payment);
        Task<Payment> CapturePayment(Payment payment, TransactionUpdateDetails details);
        Task<Payment> RefundPayment(Payment payment, TransactionUpdateDetails details);
    }
}
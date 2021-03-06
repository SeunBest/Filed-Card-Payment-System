using FiledCard.Core.Entities;
using System.Threading.Tasks;

namespace FiledCard.Core.IServices
{
    public interface ICheapPaymentGatewayService
    {
        Task AddPayment(Payment payment);
        Task<Payment> ProcessPayment(string id);
        Task<Payment> FailedPayment(string id);
    }
}

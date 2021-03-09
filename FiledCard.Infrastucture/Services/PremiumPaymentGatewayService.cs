using FiledCard.Core.Entities;
using FiledCard.Core.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FiledCard.Infrastucture.Services
{
    public class PremiumPaymentGatewayService : IPremiumPaymentGatewayService
    {
        private readonly AppDbContext _ctx;

        public PremiumPaymentGatewayService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task AddPayment(Payment payment)
        {
            await _ctx.Payments.AddAsync(payment);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Payment> ProcessPayment(string id)
        {
            var payment = _ctx.Payments.Where(x => x.PaymentId == id)
                              .Include(x => x.PaymentState).FirstOrDefault();

            payment.PaymentState.State = "processed";
            _ctx.Update(payment.PaymentState);
            await _ctx.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> FailedPayment(string id)
        {
            var payment = _ctx.Payments.Where(x => x.PaymentId == id)
                              .Include(x => x.PaymentState).FirstOrDefault();

            payment.PaymentState.State = "failed";
            _ctx.Update(payment);
            await _ctx.SaveChangesAsync();
            return payment;
        }
    }
}

using FiledCard.Core.Entities;
using FiledCard.Core.IServices;
using System.Threading.Tasks;

namespace FiledCard.Infrastucture.Services
{
    public class ProcessPaymentService
    {
        private readonly ICheapPaymentGatewayService _cheapPaymentGateway;
        private readonly IExpensivePaymentGatewayService _expensivePaymentGatewayService;
        private readonly IPremiumPaymentGatewayService _premiumPaymentGatewayService;

        public ProcessPaymentService(ICheapPaymentGatewayService cheapPaymentGateway, 
            IExpensivePaymentGatewayService expensivePaymentGatewayService, 
            IPremiumPaymentGatewayService premiumPaymentGatewayService)
        {
            _cheapPaymentGateway = cheapPaymentGateway;
            _expensivePaymentGatewayService = expensivePaymentGatewayService;
            _premiumPaymentGatewayService = premiumPaymentGatewayService;
        }

        public async Task<Payment> ProcessPayment(Payment payment)
        {
            if (payment.Amount <= 20)
            {
                await _cheapPaymentGateway.AddPayment(payment);
                return await _cheapPaymentGateway.ProcessPayment(payment.PaymentId);
            } 
            else if (payment.Amount > 20 && payment.Amount <= 500)
            {
                await _expensivePaymentGatewayService.AddPayment(payment);
                var response = await _expensivePaymentGatewayService.ProcessPayment(payment.PaymentId);

                if (response.PaymentState.State == "processed")
                    return response;

                response = await _cheapPaymentGateway.ProcessPayment(payment.PaymentId);
                if (response.PaymentState.State == "processed")
                    return response;
            } 
            else if (payment.Amount > 500)
            {
                await _premiumPaymentGatewayService.AddPayment(payment);
                int i = 0;
                while(i < 3)
                {
                    var response = await _premiumPaymentGatewayService.ProcessPayment(payment.PaymentId);

                    if (response.PaymentState.State == "processed")
                        return response;
                    i++;
                }
            }
            return await _cheapPaymentGateway.FailedPayment(payment.PaymentId);
        }
    }
}

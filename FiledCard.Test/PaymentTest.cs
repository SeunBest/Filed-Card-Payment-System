using FiledCard.Core.DTOs;
using FiledCard.Core.Entities;
using FiledCard.Core.IServices;
using FiledCard.Infrastucture.Services;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace FiledCard.Test
{
    public class PaymentTest
    {
        [Fact]
        public async Task ShouldProcessCheapPaymentCorrectly()
        {
            var cheapPaymentGatewayServiceMock = new Mock<ICheapPaymentGatewayService>();
            var expensivePaymentGatewayServiceMock = new Mock<IExpensivePaymentGatewayService>();
            var premiumPaymentGatewayServiceMock = new Mock<IPremiumPaymentGatewayService>();

            var request = new PaymentDto()
            {
                CardHolder = "Linda Swan",
                CreditCardNumber = "4012888888881881",
                ExpirationDate = DateTime.Now.AddYears(3),
                SecurityCode = "545",
                Amount = 15
            };

            var payment = new Payment()
            {
                PaymentId = "awqwqwd12311",
                CardHolder = request.CardHolder,
                CreditCardNumber = request.CreditCardNumber,
                ExpirationDate = request.ExpirationDate,
                SecurityCode = request.SecurityCode,
                Amount = request.Amount,
                PaymentState = new PaymentState()
                {
                    PaymentStateId = "we32ew",
                    State = "pending"
                }
            };

            cheapPaymentGatewayServiceMock.Setup(p => p.AddPayment(payment));
            payment.PaymentState.State = "processed";
            cheapPaymentGatewayServiceMock.Setup(p => p.ProcessPayment(payment.PaymentId)).ReturnsAsync(payment);

            var processPaymentService = new ProcessPaymentService(cheapPaymentGatewayServiceMock.Object, expensivePaymentGatewayServiceMock.Object, premiumPaymentGatewayServiceMock.Object);

            var result = await processPaymentService.ProcessPayment(payment);
            Assert.Equal("processed", result.PaymentState.State);

            cheapPaymentGatewayServiceMock.Verify(e => e.FailedPayment(payment.PaymentId), Times.Never);

            expensivePaymentGatewayServiceMock.Verify(e => e.AddPayment(payment), Times.Never);
            expensivePaymentGatewayServiceMock.Verify(e => e.ProcessPayment(payment.PaymentId), Times.Never);
            expensivePaymentGatewayServiceMock.Verify(e => e.FailedPayment(payment.PaymentId), Times.Never);

            premiumPaymentGatewayServiceMock.Verify(e => e.AddPayment(payment), Times.Never);
            premiumPaymentGatewayServiceMock.Verify(e => e.ProcessPayment(payment.PaymentId), Times.Never);
            premiumPaymentGatewayServiceMock.Verify(e => e.FailedPayment(payment.PaymentId), Times.Never);
        }
    }
}

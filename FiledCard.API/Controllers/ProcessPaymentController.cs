using AutoMapper;
using FiledCard.Core.DTOs;
using FiledCard.Core.Entities;
using FiledCard.Core.Helpers;
using FiledCard.Core.IServices;
using FiledCard.Infrastucture.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FiledCard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessPaymentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICheapPaymentGatewayService _cheapPaymentGatewayService;
        private readonly IExpensivePaymentGatewayService _expensivePaymentGatewayService;
        private readonly IPremiumPaymentGatewayService _premiumPaymentGatewayService;

        public ProcessPaymentController(IServiceProvider provider)
        {
            _mapper = provider.GetRequiredService<IMapper>();
            _cheapPaymentGatewayService = provider.GetRequiredService<ICheapPaymentGatewayService>();
            _expensivePaymentGatewayService = provider.GetRequiredService<IExpensivePaymentGatewayService>();
            _premiumPaymentGatewayService = provider.GetRequiredService<IPremiumPaymentGatewayService>();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (VerifyCard.IsValidCreditCardNumber(model.CreditCardNumber) && VerifyCard.IsNotExpired(model.ExpirationDate))
                    {
                        var paymentService = new ProcessPaymentService(_cheapPaymentGatewayService, _expensivePaymentGatewayService, _premiumPaymentGatewayService);

                        var payment = _mapper.Map<Payment>(model);
                        payment.PaymentId = Guid.NewGuid().ToString();

                        payment.PaymentState = new PaymentState
                        {
                            PaymentStateId = Guid.NewGuid().ToString(),
                            State = "pending"
                    };
                        
                        var response = await paymentService.ProcessPayment(payment);

                        if (response.PaymentState.State == "processed")
                            return Ok(response);

                        return StatusCode(500, "Payment failed");
                    }
                }
                return BadRequest("The Request is invalid");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

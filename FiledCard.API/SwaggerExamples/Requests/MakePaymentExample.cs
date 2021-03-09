using FiledCard.Core.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace FiledCard.API.SwaggerExamples.Requests
{
    public class MakePaymentExample : IExamplesProvider<PaymentDto>
    {
        public PaymentDto GetExamples()
        {
            return new PaymentDto()
            {
                CardHolder = "Samuel Blake",
                CreditCardNumber = "371449635398431",
                SecurityCode = "890",
                ExpirationDate = DateTime.Now.AddYears(3),
                Amount = 350
            };
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FiledCard.Core.DTOs
{
    public class PaymentDto
    {
        [Required]
        public string CreditCardNumber { get; set; }

        [Required]
        public string CardHolder { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }

        [StringLength(3, MinimumLength = 3, ErrorMessage = "The Security Code should be 3 digits")]
        public string SecurityCode { get; set; }

        [Required]
        [Range(0.1, Double.MaxValue, ErrorMessage = "The amount {0} has to be a positive number")]
        public decimal Amount { get; set; }
    }
}

using FiledCard.Core.DTOs;

namespace FiledCard.Core.Entities
{
    public class Payment : PaymentDto
    {
        public string PaymentId { get; set; }
        public string PaymentStateId { get; set; }
        public PaymentState PaymentState { get; set; }
    }
}

namespace FiledCard.Core.Entities
{
    public class PaymentState
    {
        public string PaymentStateId { get; set; }
        public string State { get; set; } = "pending";
    }
}

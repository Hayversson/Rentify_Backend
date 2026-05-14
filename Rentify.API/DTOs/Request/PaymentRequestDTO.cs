namespace Rentify.API.DTOs.Request
{
    public class PaymentRequestDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}

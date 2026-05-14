namespace Rentify.API.DTOs.Response
{
    public class PaymentResponseDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}

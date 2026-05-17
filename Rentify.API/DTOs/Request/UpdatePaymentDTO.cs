namespace Rentify.API.DTOs.Request
{
    public class UpdatePaymentDTO
    {
        public decimal Amount { get; set; } // 
        public string Method { get; set; } = string.Empty;
    }
}

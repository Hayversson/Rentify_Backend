namespace Rentify.API.DTOs.Request
{
    public class VehicleTypeRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal PricePerDay { get; set; }
    }
}

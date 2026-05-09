
namespace Rentify.API.DTOs.Request
{
    public class VehicleRequestDTO
    {
        public string Plate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int VehicleTypeId { get; set; }
        public int BranchId { get; set; }

    }
}

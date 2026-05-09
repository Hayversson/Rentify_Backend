using Rentify.Domain.Enums;

namespace Rentify.API.DTOs.Response
{
    public class VehicleResponseDTO
    {
        public int Id { get; set; }
        public string Plate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public VehicleStatus Status { get; set; }
        public int VehicleTypeId { get; set; }
        public int BranchId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

namespace Rentify.API.DTOs.Request
{
    public class VehicleMaintenanceRequestDTO
    {
        public int VehicleId { get; set; }
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

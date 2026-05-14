namespace Rentify.API.DTOs.Response
{
    public class BranchVehicleResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool IsActive { get; set; }

        public List<VehicleResponseDTO> Vehicles { get; set; } = new();
    }
}

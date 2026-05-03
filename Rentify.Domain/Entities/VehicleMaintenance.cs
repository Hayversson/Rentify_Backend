namespace Rentify.Domain.Entities
{
    public class VehicleMaintenance : AuditBase
    {
        public int VehicleId { get; set; }
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Vehicle Vehicle { get; set; } = null!;
    }
}

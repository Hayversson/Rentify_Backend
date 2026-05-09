using Rentify.Domain.Enums;

namespace Rentify.Domain.Entities
{
    public class Vehicle : AuditBase
    {
        public string Plate { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public VehicleStatus Status { get; set; }
        public int VehicleTypeId { get; set; }
        public int BranchId { get; set; }

        // Navigation properties
        public VehicleType VehicleType { get; set; } = null!;
        public Branch Branch { get; set; } = null!;
        public ICollection<VehicleMaintenance> Maintenances { get; set; } = new List<VehicleMaintenance>();
        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}

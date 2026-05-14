using Rentify.Domain.Enums;

namespace Rentify.API.DTOs.Request
{
    public class RentalRequestDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public RentalStatus Status { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }

        public int PickupBranchId { get; set; }
        public int ReturnBranchId { get; set; }
    }
}

using Rentify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Entities
{
    public class Rental : AuditBase
    {
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }

        public int PickupBranchId { get; set; }
        public int ReturnBranchId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalCost { get; set; }
        public RentalStatus Status { get; set; }

        public Customer Customer { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;
        public Branch PickupBranch { get; set; } = null!;
        public Branch ReturnBranch { get; set; } = null!;

        public Payment Payment { get; set; } = null!;
    }
}

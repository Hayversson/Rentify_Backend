using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Entities
{
    public class Payment : AuditBase
    {
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;

        // FK
        public int RentalId { get; set; }

        //Navigation property
        public Rental Rental { get; set; } = null!;
    }
}

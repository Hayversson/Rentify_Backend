using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Entities
{
    public class Payment : AuditBase
    {
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;

        public Rental Rental { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Entities
{
    public class Customer : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}

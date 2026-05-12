namespace Rentify.Domain.Entities
{
        public class Branch : AuditBase
        {
            public string Name { get; set; } = string.Empty;

            public string City { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;

            public string Phone { get; set; } = string.Empty;

            public TimeSpan OpeningTime { get; set; }
            public TimeSpan ClosingTime { get; set; }

            public bool IsActive { get; set; } = true; // Branches can be deactivated (for a while) instead of deleted or to preserve historical data

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        }
    
}

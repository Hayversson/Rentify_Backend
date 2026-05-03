namespace Rentify.Domain.Entities
{
    public class Branch : AuditBase
    {
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}

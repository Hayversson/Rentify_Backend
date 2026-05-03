namespace Rentify.Domain.Entities;

public class VehicleType : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public decimal PricePerDay { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
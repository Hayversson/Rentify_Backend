using Rentify.Domain.Entities;

namespace Rentify.Domain.Interfaces.Repositories
{
    public interface IVehicleTypeRepository : IGenericRepository<VehicleType>
    {
        Task<VehicleType?> GetByNameAsync(string name);
        Task<VehicleType?> GetByPricePerDayAsync(decimal pricePerDay);
    }
}

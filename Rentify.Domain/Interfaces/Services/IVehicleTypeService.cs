using Rentify.Domain.Entities;


namespace Rentify.Domain.Interfaces.Services
{
    public interface IVehicleTypeService
    {
        Task<IEnumerable<VehicleType>> GetAllAsync();
        Task<VehicleType?> GetByIdAsync(int id);
        Task<VehicleType> CreateAsync(VehicleType vehicleType);
        Task UpdateAsync(int id, VehicleType vehicleType);
        Task DeleteAsync(int id);
    }
}

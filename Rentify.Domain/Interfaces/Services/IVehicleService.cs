using Rentify.Domain.Entities;
using Rentify.Domain.Enums;

namespace Rentify.Domain.Interfaces.Services
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task UpdateAsync(int id, Vehicle vehicle);
        Task DeleteAsync(int id);
        Task UpdateStatusAsync(int id, VehicleStatus newStatus);
    }
}

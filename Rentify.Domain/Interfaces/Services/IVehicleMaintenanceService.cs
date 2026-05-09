using Rentify.Domain.Entities;


namespace Rentify.Domain.Interfaces.Services
{
    public interface IVehicleMaintenanceService
    {
        Task<IEnumerable<VehicleMaintenance>> GetAllAsync();
        Task<VehicleMaintenance?> GetByIdAsync(int id);
        Task<VehicleMaintenance> CreateAsync(VehicleMaintenance vehicleMaintenance);
        Task UpdateAsync(int id, VehicleMaintenance vehicleMaintenance);
        Task DeleteAsync(int id);
    }
}

using Rentify.Domain.Entities;
using Rentify.Domain.Enums;


namespace Rentify.Domain.Interfaces.Repositories
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status);
        Task<Vehicle?> GetByIdWithVehicleTypeAndMaintenanceAsync(int id);
        Task<Vehicle?> GetByPlateAsync(string plate);
        Task<Vehicle?> GetByYearAsync(int year);
        Task<IEnumerable<Vehicle>> GetByTypeIdAsync(int vehicleTypeId);
    }
}

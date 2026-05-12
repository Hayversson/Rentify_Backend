using Microsoft.EntityFrameworkCore;
using Rentify.DataAccess.Context;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Repositories;


namespace Rentify.DataAccess.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(RentifyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status)
        {
            return await _dbSet
                .Where(v => v.Status == status)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetByIdWithVehicleTypeAndMaintenanceAsync(int id)
        {
            return await _dbSet
                .Include(v => v.VehicleType)
                .Include(v => v.Maintenances)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<Vehicle?> GetByPlateAsync(string plate)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.Plate == plate);
        }

        public async Task<Vehicle?> GetByYearAsync(int year)
        {
            return await _dbSet
                .FirstOrDefaultAsync(v => v.Year == year);

        }

        public async Task<IEnumerable<Vehicle>> GetByTypeIdAsync(int vehicleTypeId)
        {
            return await _dbSet
                .Where(v => v.VehicleTypeId == vehicleTypeId)
                .ToListAsync();
        }
        

    }
}

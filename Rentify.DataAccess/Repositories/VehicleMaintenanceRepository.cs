using Rentify.DataAccess.Context;
using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Rentify.DataAccess.Repositories
{
    public class VehicleMaintenanceRepository : GenericRepository<VehicleMaintenance>, IVehicleMaintenanceRepository
    {
        public VehicleMaintenanceRepository(RentifyDbContext context) : base(context)
        {
        }

        public async Task<VehicleMaintenance?> GetByDescriptionAsync(string description) {
            return await _dbSet
                .FirstOrDefaultAsync(vm => vm.Description.ToLower() == description.ToLower());
        }
    }
}

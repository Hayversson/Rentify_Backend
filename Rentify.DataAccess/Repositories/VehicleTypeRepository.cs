using Rentify.DataAccess.Context;
using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Rentify.DataAccess.Repositories
{
    public class VehicleTypeRepository : GenericRepository<VehicleType>, IVehicleTypeRepository
    {
        public VehicleTypeRepository(RentifyDbContext context) : base(context)
        {
        }

        public async Task<VehicleType?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(vt => vt.Name.ToLower() == name.ToLower());
        }

        public async Task<VehicleType?> GetByPricePerDayAsync(decimal pricePerDay)
        {
            return await _dbSet
                .FirstOrDefaultAsync(vt => vt.PricePerDay == pricePerDay);
        }
    }
}

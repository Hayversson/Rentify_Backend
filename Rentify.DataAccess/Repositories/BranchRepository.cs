using Microsoft.EntityFrameworkCore;
using Rentify.DataAccess.Context;
using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Repositories;

namespace Rentify.DataAccess.Repositories
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(RentifyDbContext context) : base(context)
        {
        }

        public async Task<Branch?> GetByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.Name == name);
        }

        public async Task<IEnumerable<Branch>> GetActiveBranchesAsync()
        {
            return await _dbSet
                .Where(b => b.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Branch>> GetWithVehiclesAsync()
        {
            return await _dbSet
                .Include(b => b.Vehicles)
                .ToListAsync();
        }

        public async Task<Branch?> GetByIdWithVehiclesAsync(int id)
        {
            return await _dbSet
                .Include(b => b.Vehicles)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _dbSet
                .AnyAsync(b => b.Name == name);
        }
    }
}

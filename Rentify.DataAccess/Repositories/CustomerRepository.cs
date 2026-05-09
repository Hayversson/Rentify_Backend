using Microsoft.EntityFrameworkCore;
using Rentify.DataAccess.Context;
using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Repositories;

namespace Rentify.DataAccess.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(RentifyDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetByLicenseNumberAsync(string licenseNumber)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.LicenseNumber == licenseNumber);
        }

        public async Task<IEnumerable<Customer>> GetWithRentalsAsync()
        {
            return await _dbSet
                .Include(c => c.Rentals)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdWithRentalsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Rentals)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(c => c.Email == email);
        }

        public async Task<bool> LicenseExistsAsync(string licenseNumber)
        {
            return await _dbSet
                .AnyAsync(c => c.LicenseNumber == licenseNumber);
        }
    }
}

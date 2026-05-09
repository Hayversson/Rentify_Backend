using Rentify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetByLicenseNumberAsync(string licenseNumber);

        Task<IEnumerable<Customer>> GetWithRentalsAsync();
        Task<Customer?> GetByIdWithRentalsAsync(int id);

        Task<bool> EmailExistsAsync(string email);
        Task<bool> LicenseExistsAsync(string licenseNumber);
    }
}

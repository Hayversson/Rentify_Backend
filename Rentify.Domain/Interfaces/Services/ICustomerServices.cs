using Rentify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);

        Task<Customer?> GetByIdWithRentalsAsync(int id);

        Task<Customer> CreateAsync(Customer customer);
        Task UpdateAsync(int id, Customer customer);

        Task DeleteAsync(int id);

    }
}

using Rentify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Services
{
    public interface IRentalServices
    {
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(int id);
        Task<Rental> CreateAsync(Rental entity);
        Task UpdateAsync(Rental entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}

using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Services
{
    public interface IRentalServices
    {
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(int id);
        Task<Rental> CreateAsync(Rental entity, string paymentMethod);
        Task UpdateAsync(Rental rental);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Rental?>> GetByVehiculeAsync(int vehicleId);
        Task<IEnumerable<Rental?>> GetByCustomerAsync(int customerId);
        Task UpdateStatusAsync(int id, RentalStatus newStatus);
    }
}

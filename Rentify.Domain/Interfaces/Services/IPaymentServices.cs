using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Services
{
    public interface IPaymentServices
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment> CreateAsync(Payment entity);
        Task UpdateAsync(Payment entity, int id);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Payment>> GetByCustomer(int id);
        Task<IEnumerable<Payment>> GetByVehicule(int id);
    }
}

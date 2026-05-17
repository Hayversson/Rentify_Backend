using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentRepository _rep;
        public PaymentServices(IPaymentRepository rep)
        {
            _rep = rep;
        }
        public async Task<Payment> CreateAsync(Payment entity)
        {
            return await _rep.CreateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _rep.DeleteAsync(id);
        }

        public Task<bool> ExistsAsync(int id)
        {
            return _rep.ExistsAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _rep.GetAllAsync();
        }

        public async Task<IEnumerable<Payment>> GetByCustomer(int id)
        {
            return await _rep.GetByCustomer(id);
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _rep.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetByVehicule(int id)
        {
            return await _rep.GetByVehicule(id);
        }

        public async Task UpdateAsync(Payment entity, int id)
        {
            var existing = await _rep.GetByIdAsync(id);
            if (existing is null)
            {
                throw new KeyNotFoundException($"Payment with id {id} not found.");
            }

            existing.Amount = entity.Amount;
            existing.Method = entity.Method;

            await _rep.UpdateAsync(existing);
        }
    }
}

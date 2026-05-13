using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Services
{
    public class RentalServices : IRentalServices
    {
        private readonly IRentalRepository _rep;
        public RentalServices(IRentalRepository rep)
        {
            _rep = rep;
        }
        public async Task<Rental> CreateAsync(Rental entity)
        {
            return await _rep.CreateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _rep.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _rep.ExistsAsync(id);
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _rep.GetAllAsync();
        }

        public async Task<IEnumerable<Rental?>> GetByCustomerAsync(int customerId)
        {
            return await _rep.GetByCustomerAsync(customerId);
        }

        public async Task<Rental?> GetByIdAsync(int id)
        {
            return await _rep.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Rental?>> GetByVehiculeAsync(int vehicleId)
        {
            return await _rep.GetByVehiculeAsync(vehicleId);
        }

        public async Task UpdateRentalAsync(int id, Rental rental)
        {
            await _rep.UpdateRentalAsync(id, rental);
        }
    }
}

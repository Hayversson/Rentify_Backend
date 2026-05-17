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
        public async Task<Rental> CreateAsync(Rental entity, string paymentMethod)
        {
            if (entity.Vehicle.Status != VehicleStatus.Available || entity.StartDate > entity.EndDate ||
                entity.StartDate < DateTime.Now)
            {
                throw new InvalidOperationException("The vehicle is not available for rental.");
            }
            int days = (entity.EndDate - entity.StartDate).Days;
            entity.TotalCost = days * entity.Vehicle.VehicleType.PricePerDay;
            var payment = new Payment
            {
                Amount = entity.TotalCost,
                Method = paymentMethod,
                RentalId = entity.Id,
            };
            entity.Payment = payment;
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

        public async Task UpdateAsync(Rental rental, int id)
        {
            var existingRental = await _rep.GetByIdAsync(id);
            if (existingRental == null)
            {
                throw new InvalidOperationException("Rental not found.");
            }

            existingRental.StartDate = rental.StartDate;
            existingRental.EndDate = rental.EndDate;
            existingRental.PickupBranchId = rental.PickupBranchId;
            existingRental.ReturnBranchId = rental.ReturnBranchId;
            existingRental.VehicleId = rental.VehicleId;
            existingRental.CustomerId = rental.CustomerId;
            existingRental.UpdatedAt = DateTime.Now;

            await _rep.UpdateAsync(existingRental);
        }

        public async Task UpdateStatusAsync(int id, RentalStatus newStatus)
        {
            await _rep.UpdateStatusAsync(id, newStatus);
        }
    }
}

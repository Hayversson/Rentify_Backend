using Microsoft.EntityFrameworkCore;
using Rentify.DataAccess.Context;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.DataAccess.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        public RentalRepository(RentifyDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Rental?>> GetByCustomerAsync(int customerId)
        {
            if (!await ExistsAsync(customerId))
            {
                throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
            }
            var rentals = await _dbSet.Where(c => c.CustomerId == customerId).ToListAsync();
            return rentals;
        }
        public async Task UpdateStatusAsync(int id, RentalStatus newStatus)
        {
            var entity = await GetByIdAsync(id);
            if (entity is null)
            {
                throw new KeyNotFoundException($"Rental with ID {id} not found.");
            }
            var validtransitions = (entity.Status, newStatus) switch
            {
                (RentalStatus.Pending, RentalStatus.Active) => true,
                (RentalStatus.Pending, RentalStatus.Cancelled) => true,
                (RentalStatus.Active, RentalStatus.Completed) => true,
                _ => false
            };
            if (!validtransitions)
            {
                throw new InvalidOperationException($"Invalid status transition from {entity.Status} to {newStatus}.");
            }
            entity.Status = newStatus;
            await UpdateAsync(entity);
        }

        public async Task<IEnumerable<Rental?>> GetByVehiculeAsync(int vehicleId)
        {
            if (!await ExistsAsync(vehicleId))
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }
            var rentals = await _dbSet.Where(c => c.VehicleId == vehicleId).ToListAsync();
            return rentals;
        }
    }
}

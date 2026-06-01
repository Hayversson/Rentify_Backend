using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Repositories
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<IEnumerable<Rental?>> GetByVehiculeAsync(int vehicleId);
        Task<IEnumerable<Rental?>> GetByCustomerAsync(int customerId);
        Task UpdateStatusAsync(int id, RentalStatus newstatus);
    }
}

using Rentify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Repositories
{
    public interface IVehicleMaintenanceRepository : IGenericRepository<VehicleMaintenance>
    {
        Task<VehicleMaintenance?> GetByDescriptionAsync(string description);
    }
}

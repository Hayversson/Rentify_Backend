using Rentify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Repositories
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<Branch?> GetByNameAsync(string name);

        Task<IEnumerable<Branch>> GetActiveBranchesAsync();

        Task<IEnumerable<Branch>> GetWithVehiclesAsync();
        Task<Branch?> GetByIdWithVehiclesAsync(int id);

        Task<bool> NameExistsAsync(string name);
    }
}

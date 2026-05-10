using Rentify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Interfaces.Services
{
    public interface IBranchService
    {
        Task<IEnumerable<Branch>> GetAllAsync();
        Task<Branch?> GetByIdAsync(int id);

        Task<IEnumerable<Branch>> GetActiveBranchesAsync();
        Task<Branch?> GetByIdWithVehiclesAsync(int id);

        Task<Branch> CreateAsync(Branch branch);
        Task UpdateAsync(int id, Branch branch);

        Task DeleteAsync(int id);

        Task DeactivateAsync(int id);

        Task ActivateAsync(int id);
    }
}

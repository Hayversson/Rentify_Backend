using Microsoft.Extensions.Logging;
using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ILogger<BranchService> _logger;

        public BranchService(
            IBranchRepository branchRepository,
            ILogger<BranchService> logger)
        {
            _branchRepository = branchRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _branchRepository.GetAllAsync();
        }

        public async Task<Branch?> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            return await _branchRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Branch>> GetActiveBranchesAsync()
        {
            return await _branchRepository.GetActiveBranchesAsync();
        }

        public async Task<Branch?> GetByIdWithVehiclesAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            return await _branchRepository.GetByIdWithVehiclesAsync(id);
        }

        public async Task<Branch> CreateAsync(Branch branch)
        {
            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            await ValidateBranchAsync(branch);

            _logger.LogInformation("Creating branch: {Name}", branch.Name);

            return await _branchRepository.CreateAsync(branch);
        }

        public async Task UpdateAsync(int id, Branch branch)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            var existing = await _branchRepository.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException($"Branch with ID {id} not found");

            await ValidateBranchAsync(branch, id);

            existing.Name = branch.Name;
            existing.City = branch.City;
            existing.Address = branch.Address;
            existing.Phone = branch.Phone;
            existing.OpeningTime = branch.OpeningTime;
            existing.ClosingTime = branch.ClosingTime;

            _logger.LogInformation("Updating branch: {Id}", id);

            await _branchRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            var branch = await _branchRepository.GetByIdWithVehiclesAsync(id);

            if (branch == null)
                throw new KeyNotFoundException($"Branch with ID {id} not found");

            if (branch.Vehicles.Any())
                throw new InvalidOperationException("Cannot delete branch with assigned vehicles");

            await _branchRepository.DeleteAsync(id);
        }

        public async Task DeactivateAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
                throw new KeyNotFoundException($"Branch with ID {id} not found");

            branch.IsActive = false;

            _logger.LogInformation("Deactivating branch: {Id}", id);

            await _branchRepository.UpdateAsync(branch);
        }

        public async Task ActivateAsync(int id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);

            if (branch == null)
                throw new KeyNotFoundException("Sucursal no encontrada");

            branch.IsActive = true;

            await _branchRepository.UpdateAsync(branch);
        }

        // HELPER PRIVADO
        private async Task ValidateBranchAsync(Branch branch, int? id = null)
        {
            if (branch == null)
                throw new ArgumentNullException(nameof(branch));

            if (string.IsNullOrWhiteSpace(branch.Name))
                throw new ArgumentException("Branch name is required");

            if (string.IsNullOrWhiteSpace(branch.City))
                throw new ArgumentException("City is required");

            if (string.IsNullOrWhiteSpace(branch.Address))
                throw new ArgumentException("Address is required");

            if (branch.OpeningTime >= branch.ClosingTime)
                throw new InvalidOperationException("Opening time must be less than closing time");

            // Nombre único
            var exists = await _branchRepository.GetByNameAsync(branch.Name);
            if (exists != null && exists.Id != id)
                throw new InvalidOperationException("Branch name already exists");
        }
    }
}

using Microsoft.Extensions.Logging;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentify.Domain.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            ICustomerRepository customerRepository,
            ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Customer?> GetByIdWithRentalsAsync(int id)
        {
            return await _customerRepository.GetByIdWithRentalsAsync(id);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            // Validaciones
            await ValidateCustomerAsync(customer);

            _logger.LogInformation("Creating customer: {Email}", customer.Email);

            return await _customerRepository.CreateAsync(customer);
        }

        public async Task UpdateAsync(int id, Customer customer)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var existing = await _customerRepository.GetByIdAsync(id);

            if (existing == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            // Validar cambios
            await ValidateCustomerAsync(customer, id);

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.Email = customer.Email;
            existing.Phone = customer.Phone;
            existing.LicenseNumber = customer.LicenseNumber;
            existing.LicenseExpirationDate = customer.LicenseExpirationDate;

            _logger.LogInformation("Updating customer: {Id}", id);

            await _customerRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero");

            var customer = await _customerRepository.GetByIdWithRentalsAsync(id);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            // Validación importante
            if (customer.Rentals.Any(r => r.Status == RentalStatus.Active))
                throw new InvalidOperationException("Cannot delete customer with active rentals");

            _logger.LogInformation("Deleting customer: {Id}", id);

            await _customerRepository.DeleteAsync(id);
        }

        // HELPER PRIVADO
        private async Task ValidateCustomerAsync(Customer customer, int? id = null)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email is required");

            if (string.IsNullOrWhiteSpace(customer.FirstName))
                throw new ArgumentException("First name is required");

            if (string.IsNullOrWhiteSpace(customer.LastName))
                throw new ArgumentException("Last name is required");

            if (string.IsNullOrWhiteSpace(customer.LicenseNumber))
                throw new ArgumentException("License number is required");

            if (customer.LicenseExpirationDate <= DateTime.UtcNow)
                throw new InvalidOperationException("License is expired");

            // Email único
            var emailExists = await _customerRepository.GetByEmailAsync(customer.Email);
            if (emailExists != null && emailExists.Id != id)
                throw new InvalidOperationException("Email already exists");

            // Licencia única
            var licenseExists = await _customerRepository.GetByLicenseNumberAsync(customer.LicenseNumber);
            if (licenseExists != null && licenseExists.Id != id)
                throw new InvalidOperationException("License already exists");
        }
    }
}

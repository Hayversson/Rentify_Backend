using Microsoft.Extensions.Logging;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;

namespace Rentify.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IVehicleTypeRepository _typeRepository;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(
           IVehicleRepository vehicleRepository,
           IBranchRepository branchRepository,
           IVehicleTypeRepository typeRepository,
           ILogger<VehicleService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _branchRepository = branchRepository;
            _typeRepository = typeRepository;
            _logger = logger;
        }

        #region GetAllAsync
        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all vehicles");
            return await _vehicleRepository.GetAllAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving Vehicle with ID: {VehicleId}", id);
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleId} not found.", id);
                throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
            }
            
            return vehicle;
        }
        #endregion
        
        #region CreateAsync
        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            //	No crear vehículo con placa duplicada 
            vehicle.Plate = vehicle.Plate.Trim().ToUpper();

            var existingPlate = await _vehicleRepository.GetByPlateAsync(vehicle.Plate);
            if (existingPlate != null)
            {
                throw new InvalidOperationException(
                    $"Ya existe un vehículo con la placa '{vehicle.Plate}'");
            }

            // Validar que el año del vehículo no sea mayor al año actual
            if (vehicle.Year > DateTime.UtcNow.Year)
            {
                throw new ArgumentException("El año no puede ser mayor al actual");
            }

            // Al crear un vehículo, su estado inicial debe ser "Disponible"
            vehicle.Status = VehicleStatus.Available;

            _logger.LogInformation("Creating a new Vehicle");
            return await _vehicleRepository.CreateAsync(vehicle);
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, Vehicle vehicle)
        {
            // Validar que el vehículo exista antes de actualizar
            var existing = await _vehicleRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"No se encontró el vehículo con ID {id}");

            // Validar que la nueva rama existe
            var branchExists = await _branchRepository.ExistsAsync(vehicle.BranchId);
            if (!branchExists)
            {
                throw new KeyNotFoundException(
                    $"No se encontró la rama con ID {vehicle.BranchId}");
            }

            // Validar que el modelo no esté vacío
            if (string.IsNullOrWhiteSpace(vehicle.Model))
                throw new ArgumentException("El modelo es obligatorio");

            // Validar que el nuevo tipo existe
            var typeExists = await _typeRepository.ExistsAsync(vehicle.VehicleTypeId);
            if (!typeExists)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el tipo con ID {vehicle.VehicleTypeId}");
            }

            //Validar Placa vacia
            if (string.IsNullOrWhiteSpace(vehicle.Plate))
                throw new ArgumentException("La placa es obligatoria");

            // Validar placa único (si cambió la placa)
            var newPlate = vehicle.Plate.Trim().ToUpper();

            if (!string.Equals(existing.Plate, newPlate, StringComparison.OrdinalIgnoreCase))
            {
                var existingPlate = await _vehicleRepository.GetByPlateAsync(newPlate);

                if (existingPlate != null)
                    throw new InvalidOperationException($"Ya existe un vehículo con la placa '{newPlate}'");
            }

            // Validar que el año del vehículo no sea mayor al año actual
            if (vehicle.Year < 1886 || vehicle.Year > DateTime.UtcNow.Year)
                throw new ArgumentException("Año inválido");

            existing.BranchId = vehicle.BranchId;
            existing.Status = vehicle.Status;
            existing.VehicleTypeId = vehicle.VehicleTypeId;
            existing.Year = vehicle.Year;
            existing.Model = vehicle.Model;
            existing.Plate = newPlate;

            _logger.LogInformation("Updating Vehicle with ID: {VehicleId}", id);
            await _vehicleRepository.UpdateAsync(existing);
        }
        #endregion

        #region DeleteAsync
        public async Task DeleteAsync(int id)
        {
            var exists = await _vehicleRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el vehículo con ID {id}");
            }

            _logger.LogInformation("Deleting vehicle with ID: {VehicleId}", id);
            await _vehicleRepository.DeleteAsync(id);
        }
        #endregion

        #region UpdateStatusAsync
        public async Task UpdateStatusAsync(int id, VehicleStatus newStatus)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                throw new KeyNotFoundException($"No se encontró el vehículo con ID {id}");

            // No permitir manejar mantenimiento desde aquí
            if (newStatus == VehicleStatus.InMaintenance)
            {
                throw new InvalidOperationException(
                    "El estado 'InMaintenance' solo puede cambiarse desde el servicio de mantenimientos");
            }

            // Validar transiciones válidas
            var validTransition = (vehicle.Status, newStatus) switch
            {
                (VehicleStatus.InMaintenance, VehicleStatus.Available) => true,
                (VehicleStatus.Available, VehicleStatus.Rented) => true,
                (VehicleStatus.Rented, VehicleStatus.Available) => true,
                _ => false
            };

            if (!validTransition)
            {
                throw new InvalidOperationException(
                    $"No se puede cambiar de {vehicle.Status} a {newStatus}");
            }

            vehicle.Status = newStatus;

            _logger.LogInformation(
                "Updating vehicle {VehicleId} status to {NewStatus}",
                id, newStatus);
            await _vehicleRepository.UpdateAsync(vehicle);
        }
        #endregion

        #region GetByStatusAsync
        public async Task<IEnumerable<Vehicle>> GetByStatusAsync(VehicleStatus status)
        {
            _logger.LogInformation("Filtering vehicles by status: {Status}", status);
            return await _vehicleRepository.GetByStatusAsync(status);
        }
        #endregion

        public async Task<Vehicle?> GetByIdWithVehicleTypeAndMaintenanceAsync(int id)
        {
            var vehicle = await _vehicleRepository.GetByIdWithVehicleTypeAndMaintenanceAsync(id);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleId} not found.", id);
                throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
            }
            
            return vehicle;
        }
    }
}

using Microsoft.Extensions.Logging;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;


namespace Rentify.Domain.Services
{
    public class VehicleMaintenanceService : IVehicleMaintenanceService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleMaintenanceRepository _vehicleMaintenanceRepository;
        private readonly ILogger<VehicleMaintenanceService> _logger;

        public VehicleMaintenanceService(
           IVehicleRepository vehicleRepository,
           IVehicleMaintenanceRepository vehicleMaintenanceRepository,
           ILogger<VehicleMaintenanceService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleMaintenanceRepository = vehicleMaintenanceRepository;
            _logger = logger;
        }

        #region GetAll
        public async Task<IEnumerable<VehicleMaintenance>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all vehicle maintenances");
            return await _vehicleMaintenanceRepository.GetAllAsync();
        }
        #endregion

        #region GetById
        public async Task<VehicleMaintenance?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving VehicleMaintenance with ID: {VehicleMaintenanceId}", id);
            var maintenance = await _vehicleMaintenanceRepository.GetByIdAsync(id);
            if (maintenance == null)
                _logger.LogWarning("VehicleMaintenance with ID {VehicleMaintenanceId} not found.", id);
            
            return maintenance;
        }
        #endregion

        #region Create
        public async Task<VehicleMaintenance> CreateAsync(VehicleMaintenance VehicleMaintenance)
        {
            // Validar que el vehículo exista
            var vehicle = await _vehicleRepository.GetByIdAsync(VehicleMaintenance.VehicleId);

            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle {VehicleId} not found", VehicleMaintenance.VehicleId);
                throw new KeyNotFoundException($"No se encontró el vehículo con ID {VehicleMaintenance.VehicleId}");
            }
            // Validar que la descripción no esté vacía
            if (string.IsNullOrWhiteSpace(VehicleMaintenance.Description))
                throw new ArgumentException("La descripción es obligatoria");

            // Validar que las fechas sean coherentes
            if (VehicleMaintenance.EndDate < VehicleMaintenance.StartDate)
                throw new ArgumentException("La fecha final no puede ser menor a la inicial");

            // Validar que el vehículo no esté alquilado
            if (vehicle.Status == VehicleStatus.Rented)
            {
                throw new InvalidOperationException(
                    "No se puede poner en mantenimiento un vehículo alquilado");
            }

            var result = await _vehicleMaintenanceRepository.CreateAsync(VehicleMaintenance);

            // Actualizar el estado del vehículo a "En Mantenimiento"
            vehicle.Status = VehicleStatus.InMaintenance;

            // Guardar cambio de estado del vehículo
            await _vehicleRepository.UpdateAsync(vehicle);

            _logger.LogInformation(
                "Vehicle {VehicleId} moved to maintenance",
                VehicleMaintenance.VehicleId);

            return result;
        }
        #endregion

        #region Update
        public async Task UpdateAsync(int id, VehicleMaintenance VehicleMaintenance)
        {
            var existingMaintenance = await _vehicleMaintenanceRepository.GetByIdAsync(id);

            if (existingMaintenance == null)
                throw new KeyNotFoundException($"No se encontró el mantenimiento con ID {id}");

            // Obtener vehículo una vez y reutilizar la variable para evitar sombras de variable
            var vehicle = await _vehicleRepository.GetByIdAsync(VehicleMaintenance.VehicleId);

            // Validar vehículo existe (si cambió)
            if (existingMaintenance.VehicleId != VehicleMaintenance.VehicleId)
            {
                if (vehicle == null)
                {
                    throw new KeyNotFoundException(
                        $"No se encontró el vehículo con ID {VehicleMaintenance.VehicleId}");
                }
            }

            // Validar fechas
            if (VehicleMaintenance.EndDate < VehicleMaintenance.StartDate)
                throw new ArgumentException("La fecha final no puede ser menor a la inicial");

            if (string.IsNullOrWhiteSpace(VehicleMaintenance.Description))
                throw new ArgumentException("La descripción es obligatoria");

            //Validar que el vehículo no esté alquilado
            if (vehicle != null && vehicle.Status == VehicleStatus.Rented)
            {
                throw new InvalidOperationException(
                    "No se puede asignar mantenimiento a un vehículo alquilado");
            }

            // Actualizar campos
            existingMaintenance.VehicleId = VehicleMaintenance.VehicleId;
            existingMaintenance.Description = VehicleMaintenance.Description;
            existingMaintenance.StartDate = VehicleMaintenance.StartDate;
            existingMaintenance.EndDate = VehicleMaintenance.EndDate;

            await _vehicleMaintenanceRepository.UpdateAsync(existingMaintenance);
        }
        #endregion

        #region Delete
        public async Task DeleteAsync(int id)
        {
            // Validar que el mantenimiento exista
            var existingMaintenance = await _vehicleMaintenanceRepository.GetByIdAsync(id);
            if (existingMaintenance == null)
            {
                _logger.LogWarning("VehicleMaintenance with ID {VehicleMaintenanceId} not found for deletion.", id);
                throw new KeyNotFoundException($"No se encontró el mantenimiento con ID {id}");
            }

            // Obtener el vehículo asociado y actualizar su estado a disponible
            var vehicle = await _vehicleRepository.GetByIdAsync(existingMaintenance.VehicleId);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle {VehicleId} not found when deleting maintenance {MaintenanceId}", existingMaintenance.VehicleId, id);
                throw new KeyNotFoundException($"No se encontró el vehículo con ID {existingMaintenance.VehicleId}");
            }

            vehicle.Status = VehicleStatus.Available;
            await _vehicleRepository.UpdateAsync(vehicle);
            await _vehicleMaintenanceRepository.DeleteAsync(id);
        }
        #endregion

    }
}

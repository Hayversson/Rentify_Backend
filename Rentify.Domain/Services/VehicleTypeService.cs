using Microsoft.Extensions.Logging;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;

namespace Rentify.Domain.Services
{
    public class VehicleTypeService : IVehicleTypeService 
    {
        private readonly ILogger<VehicleTypeService> _logger;
        private readonly IVehicleTypeRepository _typeRepository;
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleTypeService(
           IVehicleRepository vehicleRepository,
           IVehicleTypeRepository typeRepository,
           ILogger<VehicleTypeService> logger)
        {
            _vehicleRepository = vehicleRepository;
            _typeRepository = typeRepository;
            _logger = logger;
        }
        #region GetAllAsync
        public async Task<IEnumerable<VehicleType>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all vehicle types");
            return await _typeRepository.GetAllAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<VehicleType?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving VehicleType with ID: {VehicleTypeId}", id);
            var vehicleType = await _typeRepository.GetByIdAsync(id);
            if (vehicleType == null)
                _logger.LogWarning("VehicleType with ID {VehicleTypeId} not found.", id);
            
            return vehicleType;
        }
        #endregion

        #region CreateAsync
        public async Task<VehicleType> CreateAsync(VehicleType vehicleType)
        {

            // Validación de negocio: nombre único
            var existingVehicleType = await _typeRepository.GetByNameAsync(vehicleType.Name);
            if (existingVehicleType != null)
            {
                _logger.LogWarning("VehicleType with name '{VehicleTypeName}' already exists", vehicleType.Name);
                throw new InvalidOperationException(
                    $"Ya existe un tipo de vehículo con el nombre '{vehicleType.Name}'");
            }

            //Validación de negocio: precio por día positivo
            if (vehicleType.PricePerDay <= 0)
            {
                _logger.LogWarning("Invalid PricePerDay value: {PricePerDay}", vehicleType.PricePerDay);

                throw new ArgumentException(
                    "El precio por día debe ser mayor que cero",
                    nameof(vehicleType.PricePerDay));
            }

            return await _typeRepository.CreateAsync(vehicleType);
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, VehicleType vehicleType)
        {
            // Validar que el tipo de vehículo exista
            var existingType = await _typeRepository.GetByIdAsync(id);
            if (existingType == null)
            {
                _logger.LogWarning("VehicleType with ID {VehicleTypeId} not found for update.", id);
                throw new KeyNotFoundException($"No se encontró el tipo de vehículo con ID {id}");
            }

            //Validar que el nombre no esté vacío o solo contenga espacios
            if (string.IsNullOrWhiteSpace(vehicleType.Name))
            {
                throw new ArgumentException("El nombre es obligatorio", nameof(vehicleType.Name));
            }

            vehicleType.Name = vehicleType.Name.Trim();

            // Validar nombre único (si cambió)
            if (!string.Equals(existingType.Name, vehicleType.Name, StringComparison.OrdinalIgnoreCase))
            {
                var typeWithSameName = await _typeRepository.GetByNameAsync(vehicleType.Name);
                if (typeWithSameName != null)
                {
                    throw new InvalidOperationException(
                        $"Ya existe un tipo de vehículo con el nombre '{vehicleType.Name}'");
                }
            }

            // Validar precio por día positivo (si cambió)
            if (vehicleType.PricePerDay <= 0)
            {
                throw new ArgumentException(
                    "El precio por día debe ser mayor que cero",
                    nameof(vehicleType.PricePerDay));
            }

            existingType.Name = vehicleType.Name;
            existingType.PricePerDay = vehicleType.PricePerDay;
            await _typeRepository.UpdateAsync(existingType);
        }
        #endregion

        #region DeleteAsync
        public async Task DeleteAsync(int id)
        {
            // Validar que el tipo de vehículo exista
            var existingType = await _typeRepository.GetByIdAsync(id);
            if (existingType == null)
            {
                throw new KeyNotFoundException($"No se encontró el tipo de vehículo con ID {id}");
            }
            // Validar que no existan vehículos asociados a este tipo
            var associatedVehicles = await _vehicleRepository.GetByTypeIdAsync(id);
            if (associatedVehicles.Any())
            {
                throw new InvalidOperationException($"No se puede eliminar el tipo de vehículo con ID {id} porque tiene vehículos asociados");
            }
            await _typeRepository.DeleteAsync(id);
        }
        #endregion
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Rentify.API.DTOs.Request;
using Rentify.API.DTOs.Response;
using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Services;


namespace Rentify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleMaintenanceController : ControllerBase
    {
        private readonly IVehicleMaintenanceService _vehicleMaintenanceService;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleMaintenanceController> _logger;

        public VehicleMaintenanceController(
            IVehicleMaintenanceService vehicleMaintenanceService,
            IMapper mapper,
            ILogger<VehicleMaintenanceController> logger)
        {
            _vehicleMaintenanceService = vehicleMaintenanceService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleMaintenanceResponseDTO>>> GetAll()
        {
            var maintenances = await _vehicleMaintenanceService.GetAllAsync();
            var maintenancesDto = _mapper.Map<IEnumerable<VehicleMaintenanceResponseDTO>>(maintenances);
            return Ok(maintenancesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleMaintenanceResponseDTO>> GetById(int id)
        {
            var maintenance = await _vehicleMaintenanceService.GetByIdAsync(id);

            if (maintenance == null)
                return NotFound(new { message = $"Mantenimiento con ID {id} no encontrado" });

            var maintenanceDto = _mapper.Map<VehicleMaintenanceResponseDTO>(maintenance);
            return Ok(maintenanceDto);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleMaintenanceResponseDTO>> Create(VehicleMaintenanceRequestDTO dto)
        {
            try
            {
                var maintenance = _mapper.Map<VehicleMaintenance>(dto);
                var createdMaintenance = await _vehicleMaintenanceService.CreateAsync(maintenance);
                var responseDto = _mapper.Map<VehicleMaintenanceResponseDTO>(createdMaintenance);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = responseDto.Id },
                    responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, VehicleMaintenanceRequestDTO dto)
        {
            try
            {
                var maintenance = _mapper.Map<VehicleMaintenance>(dto);
                await _vehicleMaintenanceService.UpdateAsync(id, maintenance);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _vehicleMaintenanceService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

    }
}

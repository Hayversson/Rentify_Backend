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
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(
            IVehicleService vehicleService,
            IMapper mapper,
            ILogger<VehicleController> logger)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleResponseDTO>>> GetAll()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            var vehiclesDto = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);
            return Ok(vehiclesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleResponseDTO>> GetById(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
                return NotFound(new { message = $"Vehículo con ID {id} no encontrado" });

            var vehicleDto = _mapper.Map<VehicleResponseDTO>(vehicle);
            return Ok(vehicleDto);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleResponseDTO>> Create(VehicleRequestDTO dto)
        {
            try
            {
                var vehicle = _mapper.Map<Vehicle>(dto);
                var createdVehicle = await _vehicleService.CreateAsync(vehicle);
                var responseDto = _mapper.Map<VehicleResponseDTO>(createdVehicle);

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
        public async Task<ActionResult> Update(int id, VehicleRequestDTO dto)
        {
            try
            {
                var vehicle = _mapper.Map<Vehicle>(dto);
                await _vehicleService.UpdateAsync(id, vehicle);
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
                await _vehicleService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, UpdateVehicleStatusDTO dto)
        {
            try
            {
                await _vehicleService.UpdateStatusAsync(id, dto.Status);
                return NoContent();
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        }

    }

}

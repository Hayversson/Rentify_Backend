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
    public class VehicleTypeController : ControllerBase
    {
        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleTypeController> _logger;

        public VehicleTypeController(
            IVehicleTypeService vehicleTypeService,
            IMapper mapper,
            ILogger<VehicleTypeController> logger)
        {
            _vehicleTypeService = vehicleTypeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleTypeResponseDTO>>> GetAll()
        {
            var vehicleTypes = await _vehicleTypeService.GetAllAsync();
            var vehicleTypesDto = _mapper.Map<IEnumerable<VehicleTypeResponseDTO>>(vehicleTypes);
            return Ok(vehicleTypesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleTypeResponseDTO>> GetById(int id)
        {
            var vehicleType = await _vehicleTypeService.GetByIdAsync(id);

            if (vehicleType == null)
                return NotFound(new { message = $"Tipo de vehículo con ID {id} no encontrado" });

            var vehicleTypeDto = _mapper.Map<VehicleTypeResponseDTO>(vehicleType);
            return Ok(vehicleTypeDto);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleTypeResponseDTO>> Create(VehicleTypeRequestDTO dto)
        {
            try
            {
                var vehicleType = _mapper.Map<VehicleType>(dto);
                var createdVehicleType = await _vehicleTypeService.CreateAsync(vehicleType);
                var responseDto = _mapper.Map<VehicleTypeResponseDTO>(createdVehicleType);

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
        public async Task<ActionResult> Update(int id, VehicleTypeRequestDTO dto)
        {
            try
            {
                var vehicleType = _mapper.Map<VehicleType>(dto);
                await _vehicleTypeService.UpdateAsync(id, vehicleType);
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
                await _vehicleTypeService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/vehicles")]
        public async Task<IActionResult> GetVehiclesByType(int id)
        {
            var vehicles = await _vehicleTypeService
                .GetVehiclesByTypeAsync(id);

            var response = _mapper.Map<IEnumerable<VehicleResponseDTO>>(vehicles);

            return Ok(response);
        }

    }
}

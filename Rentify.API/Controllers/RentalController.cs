using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rentify.API.DTOs.Request;
using Rentify.API.DTOs.Response;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.Domain.Interfaces.Services;

namespace Rentify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly ILogger<Rental> _log;
        private readonly IRentalServices _rental;
        private readonly IMapper _map;
        public RentalController(IRentalServices rental, ILogger<Rental> log, IMapper map)
        {
            _rental = rental;
            _log = log;
            _map = map;
        }

        [HttpGet]
        public async Task<IActionResult> GetRentals()
        {
            try
            {
                var rentals = await _rental.GetAllAsync();
                var response = _map.Map<IEnumerable<RentalResponseDTO>>(rentals);
                _log.LogInformation("Retrieved all rentals successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while retrieving rentals.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var rentals = await _rental.GetByIdAsync(id);
                var response = _map.Map<RentalResponseDTO>(rentals);
                _log.LogInformation("Retrieved rental successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while retrieving rental.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(RentalRequestDTO dto)
        {
            try
            {
                if (dto is null)
                {
                    _log.LogWarning("Received null rental request.");
                    return BadRequest("Rental data is required.");
                }
                dto.Status = 0;
                var entity = _map.Map<Rental>(dto);
                await _rental.CreateAsync(entity);
                var response = _map.Map<RentalResponseDTO>(entity);
                _log.LogInformation("Created rental successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while retrieving rentals.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RentalRequestDTO dto)
        {
            try
            {
                var entity = await _rental.GetByIdAsync(id);
                if (entity is null)
                {
                    _log.LogWarning("Rental with id {Id} not found for update.", id);
                    return BadRequest("Rental not found.");
                }

                var rental = _map.Map<Rental>(dto);
                await _rental.UpdateAsync(rental);

                var response = _map.Map<RentalResponseDTO>(rental);
                _log.LogInformation("Updated rental successfully.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while updating rental.");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _rental.GetByIdAsync(id);
                if (entity is null)
                {
                    _log.LogWarning("Rental with id {Id} not found for deletion.", id);
                    return BadRequest("Rental not found.");
                }
                await _rental.DeleteAsync(id);
                _log.LogInformation("Deleted rental successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while deleting rental.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            try
            {
                var rentals = await _rental.GetByCustomerAsync(customerId);
                var response = _map.Map<IEnumerable<RentalResponseDTO>>(rentals);
                _log.LogInformation("Retrieved rentals for customer {CustomerId} successfully.", customerId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while retrieving rentals for customer {CustomerId}.", customerId);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("vehicule/{vehiculeId}")]
        public async Task<IActionResult> GetByVehicule(int vehiculeId)
        {
            try
            {
                var rentals = await _rental.GetByVehiculeAsync(vehiculeId);
                var response = _map.Map<IEnumerable<RentalResponseDTO>>(rentals);
                _log.LogInformation("Retrieved rentals for vehicule {VehiculeId} successfully.", vehiculeId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while retrieving rentals for vehicule {VehiculeId}.", vehiculeId);
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/Status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateRentalStatusDTO dto)
        {
            try
            {
                await _rental.UpdateStatusAsync(id, dto.Status);
                _log.LogInformation("Updated rental status successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error occurred while updating rental status.");
                return BadRequest(ex.Message);
            }
        }
    }
}

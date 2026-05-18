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
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponseDTO>>> GetAll()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                var response = _mapper.Map<IEnumerable<CustomerResponseDTO>>(customers);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDTO>> GetById(int id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                    return NotFound();

                var response = _mapper.Map<CustomerResponseDTO>(customer);
                return Ok(response);
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

        [HttpGet("{id}/rentals")]
        public async Task<ActionResult<CustomerResponseDTO>> GetByIdWithRentals(int id)
        {
            try
            {
                var customer = await _customerService.GetByIdWithRentalsAsync(id);
                if (customer == null)
                    return NotFound();

                var response = _mapper.Map<CustomerResponseDTO>(customer);
                return Ok(response);
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

        [HttpPost]
        public async Task<ActionResult<CustomerResponseDTO>> Create(CustomerRequestDTO request)
        {
            try
            {
                var customer = _mapper.Map<Customer>(request);
                var created = await _customerService.CreateAsync(customer);
                var response = _mapper.Map<CustomerResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerRequestDTO request)
        {
            try
            {
                var customer = _mapper.Map<Customer>(request);
                await _customerService.UpdateAsync(id, customer);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _customerService.DeleteAsync(id);
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
    }
}

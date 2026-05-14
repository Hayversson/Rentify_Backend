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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;

        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchResponseDTO>>> GetAll()
        {
            try
            {
                var branches = await _branchService.GetAllAsync();
                var response = _mapper.Map<IEnumerable<BranchResponseDTO>>(branches);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<BranchResponseDTO>>> GetActive()
        {
            try
            {
                var branches = await _branchService.GetActiveBranchesAsync();
                var response = _mapper.Map<IEnumerable<BranchResponseDTO>>(branches);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchResponseDTO>> GetById(int id)
        {
            try
            {
                var branch = await _branchService.GetByIdAsync(id);
                if (branch == null)
                    return NotFound();

                var response = _mapper.Map<BranchResponseDTO>(branch);
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

        [HttpGet("{id}/vehicles")]
        public async Task<ActionResult<BranchVehicleResponseDTO>> GetByIdWithVehicles(int id)
        {
            try
            {
                var branch = await _branchService.GetByIdWithVehiclesAsync(id);
                if (branch == null)
                    return NotFound();

                var response = _mapper.Map<BranchVehicleResponseDTO>(branch);
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
        public async Task<ActionResult<BranchResponseDTO>> Create([FromBody] BranchRequestDTO request)
        {
            try
            {
                var branch = _mapper.Map<Branch>(request);
                var created = await _branchService.CreateAsync(branch);
                var response = _mapper.Map<BranchResponseDTO>(created);
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
        public async Task<IActionResult> Update(int id, [FromBody] BranchRequestDTO request)
        {
            try
            {
                var branch = _mapper.Map<Branch>(request);
                await _branchService.UpdateAsync(id, branch);
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
                await _branchService.DeleteAsync(id);
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

        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                await _branchService.DeactivateAsync(id);
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

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await _branchService.ActivateAsync(id);
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

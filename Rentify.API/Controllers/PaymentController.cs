using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rentify.API.DTOs.Request;
using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Services;

namespace Rentify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _log;
        private readonly IMapper _map;
        private readonly IPaymentServices _pay;

        public PaymentController(IPaymentServices pay, ILogger<PaymentController> log, IMapper map)
        {
            _pay = pay;
            _log = log;
            _map = map;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _pay.GetAllAsync();
            if (payments == null)
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _pay.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentRequestDTO payment)
        {
            if (payment == null)
            {
                return BadRequest();
            }

            var entity = _map.Map<Payment>(payment);
            var createdPayment = await _pay.CreateAsync(entity);
            var response = _map.Map<PaymentRequestDTO>(createdPayment);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PaymentRequestDTO payment)
        {
            if (payment == null)
            {
                return BadRequest();
            }
            var entity = _map.Map<Payment>(payment);
            await _pay.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _pay.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            await _pay.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var payments = await _pay.GetByCustomer(customerId);
            if (payments == null)
            {
                return NotFound();
            }
            return Ok(payments);
        }

        [HttpGet("vehicule/{vehiculeId}")]
        public async Task<IActionResult> GetByVehicule(int vehiculeId)
        {
            var payments = await _pay.GetByVehicule(vehiculeId);
            if (payments == null)
            {
                return NotFound();
            }
            return Ok(payments);
        }
    }
}

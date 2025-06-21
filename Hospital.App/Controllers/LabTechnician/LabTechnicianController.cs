using Hospital.BL.Interface.Application.LabTechnician;
using Hospital.Dto.Application.Labtecnician;
using Hospital.Dto.Application.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.App.Controllers.LabTechnician
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabTechnicianController : ControllerBase
    {
        private readonly ILabTechnicianService _service;

        public LabTechnicianController(ILabTechnicianService service)
        {
            _service = service;
        }
        [Authorize(Roles ="LabTechnician")]
        [HttpPost("AddLabTechnician")]
        public async Task<IActionResult> AddLabTechnician([FromBody] AddLabTechnicianDto labTechnicianDto)
        {
            if (labTechnicianDto == null)
            {
                return BadRequest("Lab Technician data is required.");
            }
            try
            {
                var result = await _service.AddLabTechnicianDto(labTechnicianDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles ="LabTechnician")]
        [HttpPost("LabPayment")]
        public async Task<IActionResult> LabPayment([FromBody] PaymentDto labPaymentDto, Guid appointmentId)
        {
            if (labPaymentDto == null)
            {
                return BadRequest("Lab Payment data is required.");
            }
            try
            {
                var result = await _service.PaymentAsync(labPaymentDto, appointmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles ="LabTechnician")]
        [HttpPost("AddLabTest")]
        public async Task<IActionResult> AddLabTest([FromBody] LabTestDto labTestDto, string appointmentId)
        {
            if (labTestDto == null)
            {
                return BadRequest("Lab Test data is required.");
            }
            try
            {
                var result = await _service.AddLabTestAsync(labTestDto, appointmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}

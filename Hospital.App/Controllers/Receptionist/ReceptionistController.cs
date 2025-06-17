using Hospital.BL.Interface.Application.Receptionist;
using Hospital.Dto.Application.Patient;
using Hospital.Dto.Application.Receptionist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.App.Controllers.Receptionist
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistService _service;

        public ReceptionistController(IReceptionistService service) 
        {
            _service = service;
        }



        [Authorize(Roles = "Receptionist")]
        [HttpPost("AddReceptionistDetails")]
        public async Task<IActionResult> AddReceptionistDetailsAsync([FromBody] AddReceptionistDetailsDto addReceptionistDetailsDto)
        {
            try
            {
                if (addReceptionistDetailsDto == null)
                {
                    return BadRequest("Invalid input data.");
                }
                var result = await _service.AddReceptionistDetailAsync(addReceptionistDetailsDto);
                if (result == null)
                {
                    return NotFound("Failed to add receptionist details.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "Receptionist")]
        [HttpPost("PaymentForAppointment")]
        public async Task<IActionResult> PaymentForAppointmentAsync([FromBody] PaymentDto paymentDto, string appointmentId)
        {
            try
            {
                if (paymentDto == null)
                {
                    return BadRequest("Payment details cannot be null.");
                }
                var result = await _service.PaymentAsync(paymentDto,appointmentId);
                if (result == null)
                {
                    return NotFound("Payment processing failed.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}

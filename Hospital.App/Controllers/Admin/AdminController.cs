using Hospital.BL.Interface.Application.Admin;
using Hospital.Db.Models;
using Hospital.Dto.Application;
using Hospital.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.App.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        private readonly UserManager<AppUsers> _userManager;

        public AdminController(IAdminService service, UserManager<AppUsers> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-doctor-details")]
        public async Task<IActionResult> GetAllDoctorDetailsAsync()
        {
            var result = await _service.GetAllDoctorDetailsAsync();
            if (result == null || !result.Any())
            {
                return NotFound("No doctor details found.");
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-patient-details")]
        public async Task<IActionResult> GetAllPatientDetailsAsync()
        {
            var result = await _service.GetAllPatientDetailsAsync();
            if (result == null || !result.Any())
            {
                return NotFound("No patient details found.");
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-nurse-details")]
        public async Task<IActionResult> GetAllNurseDetailsAsync()
        {
            var result = await _service.GetAllNurseDetailsAsync();
            if (result == null || !result.Any())
            {
                return NotFound("No nurse details found.");
            }
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateAppointmentByAdmin")]
        public async Task<IActionResult> UpdateAppointmentByAdminAsync([FromBody] BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId, string doctorId)
        {
            try
            {
                if (bookAppoinmentUpdateDto == null || string.IsNullOrEmpty(appointmentId))
                {
                    return BadRequest("Invalid appointment update details or appointment ID.");
                }
                var result = await _service.UpdateAppointmentByAdminAsync(bookAppoinmentUpdateDto, appointmentId,doctorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [Authorize]

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return Unauthorized();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(string.Join(", ", errors));
            }

            return Ok("Password changed successfully.");
        }
    }
}

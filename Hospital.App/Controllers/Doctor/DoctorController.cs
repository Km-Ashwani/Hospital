using Hospital.BL.Interface.Application.Doctor;
using Hospital.Dto.Application;
using Hospital.Dto.Application.Doctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.App.Controllers.Doctor
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [Authorize(Roles ="Doctor")]
        [HttpPost("add-doctor-details")]
        public async Task<IActionResult> AddDoctorDetailsAsync(AddDoctorDetailsDto doctorDetails)
        {
            
            if (doctorDetails == null)
            {
                return BadRequest("Doctor details cannot be null");
            }
            var result = await _service.AddDoctorDetailsAsync(doctorDetails);
            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpPut("UpdateAppointmentByDoctor")]
        public async Task<IActionResult> UpdateAppointmentByDoctorAsync([FromBody] BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId)
        {
            try
            {
                if (bookAppoinmentUpdateDto == null || string.IsNullOrEmpty(appointmentId))
                {
                    return BadRequest("Invalid appointment update details or appointment ID.");
                }
                var result = await _service.UpdateAppointmentBYDoctorAsync(bookAppoinmentUpdateDto, appointmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [Authorize(Roles = "Doctor")]
        [HttpGet("GetDoctorDetailsByUserId")]
        public async Task<IActionResult> GetDoctorDetailsByUserIdAsync()
        {
            var result = await _service.GetDoctorDetailsByUserIdAsync();
            if (result == null)
            {
                return NotFound("Doctor details not found for the provided user ID.");
            }
            return Ok(result);
        }


        [Authorize(Roles ="Doctor, Admin")]
        [HttpDelete("DeleteDoctor")]
        public async Task<IActionResult> DeleteDoctorAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email and password cannot be null or empty.");
            }
            var result = await _service.DeleteDoctorAsync(email, password);
            if (!result)
            {
                return NotFound("Doctor not found or deletion failed.");
            }
            return Ok("Doctor deleted successfully.");
        }


        [Authorize(Roles = "Doctor")]
        [HttpPut("UpdateDoctorDetails")]
        public async Task<IActionResult> UpdateDoctorDetailsAsync([FromBody] DoctorDetailsDto doctorDetails)
        {
            if (doctorDetails == null)
            {
                return BadRequest("Doctor details cannot be null.");
            }
            var result = await _service.UpdateDoctorDetailsAsync(doctorDetails);
            if (result == null)
            {
                return NotFound("Doctor details not found for the provided user ID.");
            }
            return Ok(result);
        }


        [Authorize(Roles = "Doctor")]
        [HttpPost("WritePrescription")]
        public async Task<IActionResult> WritePrescriptionAsync([FromBody] WritePrescriptionDto writePrescriptionDto, string appointmentId)
        {
            if (writePrescriptionDto == null || string.IsNullOrEmpty(appointmentId))
            {
                return BadRequest("Invalid prescription details or appointment ID.");
            }
            var result = await _service.WritePrescriptionAsync(writePrescriptionDto, appointmentId);
            return Ok(result);
        }


        [Authorize(Roles = "Doctor")]
        [HttpPost("WriteMedicine")]
        public async Task<IActionResult> AddMedicinePrescriptionAsync([FromBody] List<MedicineDto> medicineDto, string prescriptionId)
        {
            if (medicineDto == null || string.IsNullOrEmpty(prescriptionId))
            {
                return BadRequest("Invalid medicine details or appointment ID.");
            }
            var result = await _service.AddMedicinePrescriptionAsync(medicineDto, prescriptionId);
            return Ok(result);
        }
    }
}

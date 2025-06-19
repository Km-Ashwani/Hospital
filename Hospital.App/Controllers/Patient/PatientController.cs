using Hospital.BL.Interface.Application.Patient;
using Hospital.Dto.Application;
using Hospital.Dto.Application.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.App.Controllers.Patient
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _PatientService;
        public PatientController(IPatientService PatientService)
        {
            _PatientService = PatientService;
        }

        [Authorize(Roles ="Patient")]
        [HttpPost("Add-patient-details")]
        public async Task<IActionResult> AddPatientDetailsAsync(AddPatientDetailsDto patientDetails)
        {
            if (patientDetails == null)
            {
                return BadRequest("Patient details cannot be null");
            }
            var result = await _PatientService.AddPatientDetailsAsync(patientDetails);
            return Ok(result);
        }


        [Authorize(Roles = "Patient")]
        [HttpPost("BookAppointment")]
        public async Task<IActionResult> BookAppointmentAsync(BookAppointmentDto bookAppointmentDto)
        {
            try
            {
                if (bookAppointmentDto == null)
                {
                    return BadRequest("Book appointment details cannot be null");
                }
                var result = await _PatientService.BookAppointmentAsync(bookAppointmentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [Authorize(Roles = "Patient")]
        [HttpGet("GetAppointmentsByUser")]
        public async Task<IActionResult> GetAppointmentsByUserAsync(string id)
        {
            try
            {
                var result = await _PatientService.GetAppointmentsByUserAsync(id);
                if (result == null)
                {
                    return NotFound("No appointments found for the user.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("GetPatientDetailsByUserId")]
        public async Task<IActionResult> GetPatientDetailsByUserIdAsync()
        {
            try
            {
                var result = await _PatientService.GetPatientDetailsByUserIdAsync();
                if (result == null)
                {
                    return NotFound("Patient details not found for the provided user ID.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Roles = "Patient,Admin")]
        [HttpDelete("DeletePatient")]
        public IActionResult DeletePatientAsync(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    return BadRequest("Password and email can't be null");
                }
                var result = _PatientService.DeletePatientAsync(email, password);
                if (result.Result)
                {
                    return Ok("Patient deleted successfully.");
                }
                else
                {
                    return NotFound("Patient not found or deletion failed.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpPut("UpdatePatientDetails")]
        public async Task<IActionResult> UpdatePatientDetailsAsync(PatientsDetailsDto patientDetails)
        {
            try
            {
                if (patientDetails == null)
                {
                    return BadRequest("Patient details cannot be null");
                }
                var result = await _PatientService.UpdatePatientDetailsAsync(patientDetails);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }


        [Authorize(Roles = "Patient,Receptionist")]
        [HttpGet("SearchDoctor")]
        public async Task<IActionResult> SearchDoctorAsync(string? name = null, string? specialization = null)
        {
            try
            {
                var result = await _PatientService.SearchDoctorAsync(name, specialization);
                if (result == null)
                {
                    return NotFound("No doctors found matching the criteria.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("GetPrescription")]
        public async Task<IActionResult> GetPrescriptionAsync(string appointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(appointmentId))
                {
                    return BadRequest("Appointment ID cannot be null or empty.");
                }
                var result = await _PatientService.GetPrescriptionAsync(appointmentId);
                if (result == null)
                {
                    return NotFound("No prescription found for the provided appointment ID.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("GetLabTest")]
        public async Task<IActionResult> GetLabTestAsync(string appointmentId)
        {
            try
            {
                if (string.IsNullOrEmpty(appointmentId))
                {
                    return BadRequest("Appointment ID cannot be null or empty.");
                }
                var result = await _PatientService.GetLabTestAsync(appointmentId);
                if (result == null)
                {
                    return NotFound("No lab test found for the provided appointment ID.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}

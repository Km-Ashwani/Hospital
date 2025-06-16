using Hospital.BL.Interface.Application.Nurse;
using Hospital.Dto.Application.Nurse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.App.Controllers.Nurse
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _service;

        public NurseController(INurseService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Nurse")]
        [HttpPost("add-nurse-details")]
        public async Task<IActionResult> AddNurseDetailsAsync(AddNurseDetailsDto nurseDetails)
        {
            if (nurseDetails == null)
            {
                return BadRequest("Nurse details cannot be null");
            }
            var result = await _service.AddNurseDetailsAsync(nurseDetails);
            return Ok(result);
        }

        [Authorize(Roles = "Nurse")]
        [HttpDelete("DeleteNurse")]
        public IActionResult DeleteNurseDetails(string email, string password)
        {
            try
            {
                var result = _service.DeleteNurseAsync(email,password);
                if (result== null)
                {
                    return NotFound("Nurse details not found for deletion.");
                }
                return Ok("Nurse details deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
            }
        }
    }
}

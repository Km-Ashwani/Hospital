using Hospital.BL.Interface.Auth;
using Hospital.Dto.Application;
using Hospital.Dto.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("Registration")]

        public async Task<IActionResult> register(RegisterDto registerDto)
        {
            try
            {
                var result = await _service.RegisterAsync(registerDto);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _service.LoginAsync(loginDto);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        [HttpPost("RoleAssign")]
        public async Task<IActionResult> RollAssign(UserDto userDto, string role)
        {
            try
            {
                var result = await _service.AssignRoleAsync(userDto.Email, role);
                if (result == null)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}

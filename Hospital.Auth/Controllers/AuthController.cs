using Hospital.BL.Interface.Application.Email;
using Hospital.BL.Interface.Auth;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Dto.Application;
using Hospital.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace Hospital.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService service, UserManager<AppUsers> userManager,IEmailService emailService)
        {
            _service = service;
            _userManager = userManager;
            _emailService = emailService;
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
                if (!result)
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


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://dummy-frontend/reset-password?email={model.Email}&token={HttpUtility.UrlEncode(token)}";

            await _emailService.SendEmailAsync(model.Email, "Reset Password",
                $"Click here to reset your password: <a href='{resetLink}'>Reset Password</a>");

            return Ok("Reset password link sent to your email and also here ."+token);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(string.Join(", ", errors));
            }

            return Ok("Password reset successfully.");
        }

        

    }
}

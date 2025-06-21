using Hospital.BL.Interface.Application.Email;
using Hospital.BL.Interface.Auth;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Dto.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Service.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtGenerate _jwtGenerate;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, UserManager<AppUsers> userManager, RoleManager<IdentityRole> roleManager, IJwtGenerate jwtGenerate, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtGenerate = jwtGenerate;
            _emailService = emailService;
        }
        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (registerDto == null)
                {
                    throw new ArgumentNullException(nameof(registerDto), "RegisterDto cannot be null");
                }
                var user = new AppUsers
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone,
                };
                var result = await _context.AppUsers.FirstOrDefaultAsync(e => e.Email.ToLower() == registerDto.Email.ToLower());
                if (result != null)
                {
                    throw new Exception("user already Exits");
                }
                var createResult = await _userManager.CreateAsync(user, registerDto.Password);
                if (!createResult.Succeeded)
                {
                    var errorMessages = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception("User creation failed: " + errorMessages);
                }

                string userEmail = registerDto.Email;
                string subject = "Registration Successful";
                string body = $@"Dear {registerDto.Email},
                              We are delighted to inform you that your registration was successfully completed. 
                              You have been assigned the role of <strong>{registerDto.Password}</strong> in our system.
                              Thank you for choosing to register with <strong>Hospital Management System</strong>. 
                              We look forward to supporting you.

                              If you have any questions or need assistance, please feel free to contact our support team.

                              Best regards,  
                              <strong>Hospital Team</strong>";

                if (string.IsNullOrEmpty(registerDto.Role) || registerDto.Role == null)
                {
                    await AssignRoleAsync(registerDto.Email, "Patient");
                    await _emailService.SendEmailAsync(userEmail, subject, body);
                    return "User created successfully with patient role ";
                }

                await _emailService.SendEmailAsync(userEmail, subject, body);

                await AssignRoleAsync(registerDto.Email, registerDto.Role);
                return "User created successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: " + ex.Message + " | " + ex.InnerException?.Message);
            }

        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(e => e.UserName.ToLower() == loginDto.Email);
            var isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (user == null || !isValid)
            {
                throw new Exception("User not found or Incorrect password");
            }
            UserDto userDto = new()
            {
                Name = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
            };

            var roles = await _userManager.GetRolesAsync(user);
            LoginResponseDto response = new()
            {
                User = userDto,
                Token = _jwtGenerate.GenerateToken(user, roles)
            };

            string userEmail = loginDto.Email;
            string subject = "Registration Successful";
            string body = $@"
                        <p>Dear <strong>{loginDto.Email}</strong>,</p>
                        <p>We noticed a successful login to your account on <strong>Hospital Management System</strong>.</p>
                        <p>If this was you, no further action is required. However, if you did not attempt to log in, please:</p>
                            <ul>
                                <li>Reset your password immediately</li>
                                <li>Or contact our support team for assistance</li>
                            </ul>
                        <p>We care about your account security and are here to help you 24/7.</p>
                        <p>Thank you for using <strong>Hospital Management System</strong>.</p>
                        <p>Best regards,<br/>
                        <strong>Hospital Team</strong></p>";

            await _emailService.SendEmailAsync(userEmail, subject, body);
            return response;
        }
        public async Task<bool> AssignRoleAsync(string email, string roleName)
        {
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(e =>( e.Email??"").ToLower() == email.ToLower());
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to create role: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        //public async Task<string> RegisterPatientAsync(PatientRegisterDto patientRegisterDto)
        //{
        //    try
        //    {
        //        if (patientRegisterDto == null)
        //            throw new ArgumentNullException(nameof(patientRegisterDto), "patientRegisterDto cannot be null");

        //        var userExists = await _userManager.FindByEmailAsync(patientRegisterDto.Email);
        //        if (userExists != null)
        //            throw new Exception("User already exists");

        //        var user = new AppUsers
        //        {
        //            UserName = patientRegisterDto.Email,
        //            Email = patientRegisterDto.Email,
        //            PhoneNumber = patientRegisterDto.Phone,
        //        };

        //        var result = await _userManager.CreateAsync(user, patientRegisterDto.Password);
        //        if (!result.Succeeded)
        //        {
        //            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
        //            throw new Exception("User creation failed: " + errorMessages);
        //        }
        //        var roleName = "Patient";
        //        if (!await _roleManager.RoleExistsAsync(roleName))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole(roleName));
        //        }

        //        // ✅ Hardcoded Role assignment
        //        await _userManager.AddToRoleAsync(user, roleName);

        //        return "Patient registered successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Something went wrong: " + ex.Message);
        //    }
        //}

    }
}

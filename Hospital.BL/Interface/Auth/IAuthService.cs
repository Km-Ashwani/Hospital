using Hospital.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> AssignRoleAsync(string email, string roleName);
        //Task<string> RegisterPatientAsync(PatientRegisterDto patientRegisterDto);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Auth
{
    public class RegisterDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string? Role { get; set; } // Optional field for role, e.g., "Admin", "User"
    }
}

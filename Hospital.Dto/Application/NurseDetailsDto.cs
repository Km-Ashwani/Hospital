using Hospital.Db.Models;
using Hospital.Dto.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application
{
    public class NurseDetailsDto:AppUserDto
    {
        public string UserId { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Qualification { get; set; }
        public int Experience { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Shift { get; set; }
        public int DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

    }
}

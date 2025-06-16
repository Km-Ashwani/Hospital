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
    public class PatientsDetailsDto:AppUserDto
    {
        //public string UserId { get; set; }

        // 🔹 Personal Information
        public string Gender { get; set; }
        public string Email { get; set; }// Male, Female, Other
        public DateTime DateOfBirth { get; set; }

        // 🔹 Contact Information
        public string Address { get; set; }

        // 🔹 Medical Details
        public string BloodGroup { get; set; }               // A+, B-, etc.
        public string KnownAllergies { get; set; }           // Optional
        public string MedicalHistory { get; set; }           // Optional summary

        // 🔹 Emergency Contact
        public string? EmergencyContactNumber { get; set; }

        // 🔹 Audit Info
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

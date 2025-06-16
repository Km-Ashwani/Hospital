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
    public class DoctorDetailsDto:AppUserDto
    {


        //public string UserId { get; set; }
        public string Email { get; set; }


        // 🔹 Personal Information             
        public string Gender { get; set; }                   // Male / Female / Other
        public string Address { get; set; }                   // Male / Female / Other
        public DateTime DateOfBirth { get; set; }            // Age calculation

        // 🔹 Professional Details
        public string Specialization { get; set; }           // e.g., Cardiologist, Dentist
        public string Qualification { get; set; }            // e.g., MBBS, MD           
        public string LicenseNumber { get; set; }            // Medical license ID

        // 🔹 Availability & Working
        public bool IsAvailable { get; set; }                // True/False


        // Optional audit info
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

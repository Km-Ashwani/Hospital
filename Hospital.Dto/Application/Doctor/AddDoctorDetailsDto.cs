using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Doctor
{
    public class AddDoctorDetailsDto
    {

        // 🔹 Personal Information             
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }                  
        public int AdharNO { get; set; }                   
        public DateTime DateOfBirth { get; set; }            

        // 🔹 Professional Details
        public string Specialization { get; set; }           // e.g., Cardiologist, Dentist
        public string Qualification { get; set; }            // e.g., MBBS, MD           
        public string LicenseNumber { get; set; }            // Medical license ID

        // 🔹 Availability & Working
        public bool IsAvailable { get; set; }
    }
}

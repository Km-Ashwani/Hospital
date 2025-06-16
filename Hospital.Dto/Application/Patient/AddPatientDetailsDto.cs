using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Patient
{
    public class AddPatientDetailsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AdharNo { get; set; }
        // 🔹 Personal Information
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        // 🔹 Contact Information
        public string Address { get; set; }

        // 🔹 Medical Details
        public string BloodGroup { get; set; }               // A+, B-, etc.
        public string KnownAllergies { get; set; }           // Optional
        public string MedicalHistory { get; set; }           // Optional summary

        // 🔹 Emergency Contact
        public string? EmergencyContactNumber { get; set; }
    }
}

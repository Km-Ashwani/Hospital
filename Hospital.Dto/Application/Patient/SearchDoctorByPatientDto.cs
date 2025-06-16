using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Patient
{
    public class SearchDoctorByPatientDto
    {
        public string Email { get; set; }


        // 🔹 Personal Information             
        public string Gender { get; set; }                  
        public string FullName { get; set; }                   

        // 🔹 Professional Details
        public string Specialization { get; set; }           // e.g., Cardiologist, Dentist
        public string Qualification { get; set; }

        public bool IsAvailable { get; set; }
    }
}

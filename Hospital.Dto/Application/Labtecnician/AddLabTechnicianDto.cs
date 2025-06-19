using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Labtecnician
{
    public class AddLabTechnicianDto
    {
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int AdharNO { get; set; }
        public DateTime DateOfBirth { get; set; }

        // 🔹 Professional Details
        public string Qualification { get; set; }            // e.g., MBBS, MD           
        public string Experience { get; set; }            // e.g., MBBS, MD           


        // 🔹 Availability & Working
        public bool IsAvailable { get; set; } = true;
    }
}

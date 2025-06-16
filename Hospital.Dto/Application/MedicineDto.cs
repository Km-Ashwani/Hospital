using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application
{
    public class MedicineDto
    { 
        public string MedicineName { get; set; }
        public string Dosage { get; set; }       // e.g. "500mg"
        public string Frequency { get; set; }    // e.g. "Twice a day"
        public int DurationInDays { get; set; }  // e.g. 5 days
        public string Instructions { get; set; } // e.g. "After meals"
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Appointment
{
    public class PrescriptionMedicine
    {
        [Key]   
        public Guid PrescriptionMedicineId { get; set; }
        public Guid PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public Prescription Prescription { get; set; }

        public string MedicineName { get; set; }
        public string Dosage { get; set; }       // e.g. "500mg"
        public string Frequency { get; set; }    // e.g. "Twice a day"
        public int DurationInDays { get; set; }  // e.g. 5 days
        public string Instructions { get; set; } // e.g. "After meals"
    }
}

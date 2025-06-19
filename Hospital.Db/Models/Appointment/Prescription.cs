using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Appointment
{
    public class Prescription
    {
        [Key]
        public Guid PrescriptionId { get; set; }
        public Guid AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public Appointments Appointment { get; set; }

        public string Diagnosis { get; set; } 
        public string Symptoms { get; set; }
        public string Advice { get; set; }
        public DateTime? FollowUpDate { get; set; }
        public bool IsLabTestRequired { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<PrescriptionMedicine> Medicines { get; set; }
    }
}

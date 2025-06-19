using Hospital.Db.Models.Doctor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Appointment
{
    public  class Appointments
    {
        [Key]
        public Guid AppointmentId { get; set; }

        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public AppUsers Doctor { get; set; }

        public string PatientId { get; set; }
        [ForeignKey("PatientId")]
        public AppUsers Patient { get; set; }


        public string? ReceptionistId { get; set; }
        [ForeignKey("ReceptionistId")]
        public AppUsers Receptionist { get; set; }

        public string? LabTechnicianId { get; set; }
        [ForeignKey("LabTechnicianId")]
        public AppUsers Labtechnician { get; set; }

        public string? AppointmentDate { get; set; } 
        public string Reason { get; set; }

        public Status? Status { get; set; }
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    }

    public enum Status{
        Pending,
        Confirm,
        Canceled,
        Completed,
    }
}

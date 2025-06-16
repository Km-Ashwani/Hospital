using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Patient
{
    public class GetAppointmentDetailsDto
    {
        public Guid AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string? AppointmentDate { get; set; }
        public string Reason { get; set; }
        public Status? Status { get; set; }
        public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}

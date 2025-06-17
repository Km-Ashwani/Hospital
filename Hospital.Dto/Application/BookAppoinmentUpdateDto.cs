using Hospital.Db.Models.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital.Dto.Application
{
    public class BookAppoinmentUpdateDto
    {
        [JsonIgnore]
        public Guid AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string? AppointmentDate { get; set; }
        public Status? Status { get; set; }
    }
}

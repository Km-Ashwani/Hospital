using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Patient
{
    public class BookAppointmentDto
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public string Reason { get; set; }
        public string? AppointmentDate { get; set; }
    }
}

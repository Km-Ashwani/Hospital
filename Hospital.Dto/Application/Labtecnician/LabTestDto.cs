using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using Hospital.Db.Models.LabTests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Labtecnician
{
    public class LabTestDto
    {
        public Guid AppointmentId { get; set; }
       
        public string PatientUserId { get; set; }

        // Doctor who prescribed the test
        public string DoctorUserId { get; set; }

        public DateTime RequestedDate { get; set; } = DateTime.Now;

        public List<LabTestItemDto> labTestItemDtos { get; set; }

        public string Remarks { get; set; } // Optional notes or instructions
    }
}

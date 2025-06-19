using Hospital.Db.Models.Appointment;
using Hospital.Db.Models.Doctor;
using Hospital.Db.Models.Patients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.LabTests
{
    public class LabTest
    {
        public Guid LabTestId { get; set; }

        // Link to Appointment (optional but useful)
        public Guid AppointmentId { get; set; }
        public Appointments Appointment { get; set; }

        // Patient for whom test is prescribed
        public string PatientUserId { get; set; }
        [ForeignKey("PatientUserId")]
        public AppUsers Patient { get; set; }

        // Doctor who prescribed the test
        public string DoctorUserId { get; set; }
        [ForeignKey("DoctorUserId")]
        public AppUsers Doctor { get; set; }

        public DateTime RequestedDate { get; set; } = DateTime.Now;

        public ICollection<LabTestItem> LabTests { get; set; }

        public string Remarks { get; set; } // Optional notes or instructions
    }
}

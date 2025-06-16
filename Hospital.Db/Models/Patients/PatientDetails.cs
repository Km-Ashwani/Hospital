using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Patients
{
    public class PatientDetails
    {
        [Key]
        public int patientId { get; set; }                          // Primary key


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUsers AppUser { get; set; }

        // 🔹 Personal Information

        // 🔹 Medical Details
        public string BloodGroup { get; set; }               // A+, B-, etc.
        public string KnownAllergies { get; set; }           // Optional
        public string MedicalHistory { get; set; }           // Optional summary

        // 🔹 Emergency Contact
        public string? EmergencyContactNumber { get; set; }

    }
}

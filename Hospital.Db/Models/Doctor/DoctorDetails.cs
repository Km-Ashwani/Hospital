using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Doctor
{
    public class DoctorDetails
    {
        [Key]
        public int doctorId { get; set; }                          // Primary Key


        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUsers AppUser { get; set; }



        // 🔹 Professional Details
        public string? Qualification { get; set; }
        public string Specialization { get; set; }           // e.g., Cardiologist, Dentist
        public string LicenseNumber { get; set; }            // Medical license ID

        // 🔹 Availability & Working
        public bool IsAvailable { get; set; }                // True/False
    }
}

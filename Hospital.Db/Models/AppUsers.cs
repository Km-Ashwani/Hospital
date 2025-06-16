using Hospital.Db.Models.Appointment;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models
{
    public  class AppUsers:IdentityUser
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? AdharNo { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<Appointments> PatientAppointments { get; set; }
        public ICollection<Appointments> DoctorAppointments { get; set; }
        public ICollection<Appointments> ReceptionistAppointments { get; set; }
    }
}

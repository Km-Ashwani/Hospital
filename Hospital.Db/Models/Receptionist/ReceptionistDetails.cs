using Hospital.Db.Models.Appointment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Receptionist
{
    public class ReceptionistDetails
    {
        [Key]
        public Guid ReceptionistId { get; set; }
        public string Qualification { get; set; }
        public int ExperienceYear { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUsers AppUser { get; set; }

        public ICollection<Appointments> Appointments { get; set; }
    }
}

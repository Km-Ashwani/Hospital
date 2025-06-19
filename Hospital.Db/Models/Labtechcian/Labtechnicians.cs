using Hospital.Db.Models.LabTests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Labtechcian
{
    public class Labtechnicians
    {
        [Key]
        public Guid LabTechnicianDetailsId { get; set; } 

        public string UserId { get; set; }  // Foreign key to AppUsers
        public AppUsers User { get; set; }

        public string Qualification { get; set; }  // e.g., B.Sc (MLT)

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        public string Experience { get; set; }  // e.g., "3 years"

    }
}

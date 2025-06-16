using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.Nurse
{
    public class NurseDetails
    {
        [Key]
        public int nurseId { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUsers AppUser { get; set; }

        public string? Qualification { get; set; }
        public int Experience { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Shift { get; set; }
        public bool IsActive { get; set; } = true;
       
        public DateTime? UpdatedAt { get; set; }
    }
}

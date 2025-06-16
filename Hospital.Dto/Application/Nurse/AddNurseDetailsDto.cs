using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Nurse
{
    public class AddNurseDetailsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AdharNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Qualification { get; set; }
        public int Experience { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Shift { get; set; }
        //public int DepartmentId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

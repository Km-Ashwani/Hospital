using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models
{
    public class Department
    {
        [Key]
        public int departmnetId { get; set; }
        public string departmnetName { get; set; }
        public string departmneDescription { get; set; }
    }
}

using Hospital.Db.Models.LabTests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Labtecnician
{
    public class LabTestItemDto
    {
        public string TestName { get; set; }           // e.g. "Complete Blood Count"
        public string Description { get; set; }        // Optional

        public LabTestStatus Status { get; set; }             // Pending, In Progress, Completed
        public string Result { get; set; }             // e.g. "Normal", "High RBC Count"
        public DateTime? ResultDate { get; set; } = DateTime.Now; // Date when result was generated
    }
}

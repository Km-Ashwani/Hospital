using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.LabTests
{
    public class LabTestItem
    {
        public Guid LabTestItemId { get; set; }

        public Guid LabTesttId { get; set; }
        [ForeignKey("LabTesttId")]
        public LabTest LabTest { get; set; }

        public string TestName { get; set; }           // e.g. "Complete Blood Count"
        public string Description { get; set; }        // Optional

        public LabTestStatus Status { get; set; }             // Pending, In Progress, Completed
        public string Result { get; set; }             // e.g. "Normal", "High RBC Count"
        public DateTime? ResultDate { get; set; }
    }
    public enum LabTestStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application
{
    public class WritePrescriptionDto
    {
        //public Guid AppointmentId { get; set; }
        public string Diagnosis { get; set; }
        public string Symptoms { get; set; }
        public string Advice { get; set; }
        public DateTime? FollowUpDate { get; set; }
    }
}

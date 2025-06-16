using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Patient
{
    public class GetPrescriptionDto:WritePrescriptionDto
    {
        public Guid AppointmentId { get; set; }
        public List<MedicineDto> Medicines { get; set; } 

    }
}

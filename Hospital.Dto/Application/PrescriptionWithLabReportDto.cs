using Hospital.Dto.Application.Labtecnician;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application
{
    public class PrescriptionWithLabReportDto
    {
        public LabTestItemDto LabReport { get; set; }
        public WritePrescriptionDto Prescription { get; set; }
        public List<MedicineDto> Medicines { get; set; }
    }
}

using Hospital.Dto.Application.Labtecnician;
using Hospital.Dto.Application.Patient;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Application.LabTechnician
{
    public interface ILabTechnicianService
    {
        Task<AddLabTechnicianDto> AddLabTechnicianDto(AddLabTechnicianDto labTechnicianDto);
        Task<PaymentDto> PaymentAsync(PaymentDto paymentDto, Guid appointmentId);
        Task<LabTestDto> AddLabTestAsync(LabTestDto labTestDto, string appointmentId);
    }
}

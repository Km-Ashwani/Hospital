using Hospital.Dto.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Application.Admin
{
    public interface IAdminService
    {
        Task<List<DoctorDetailsDto>> GetAllDoctorDetailsAsync();
        Task<List<NurseDetailsDto>> GetAllNurseDetailsAsync();
        Task<List<PatientsDetailsDto>> GetAllPatientDetailsAsync();
        Task<BookAppoinmentUpdateDto> UpdateAppointmentByAdminAsync(BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId);

    }
}

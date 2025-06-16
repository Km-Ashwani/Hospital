using Hospital.Dto.Application;
using Hospital.Dto.Application.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Application.Patient
{
    public interface IPatientService
    {
        Task<AddPatientDetailsDto> AddPatientDetailsAsync(AddPatientDetailsDto patientDetails);
        Task<PatientsDetailsDto> UpdatePatientDetailsAsync(PatientsDetailsDto patientDetails);
        Task<PatientsDetailsDto> GetPatientDetailsByUserIdAsync();
        Task<bool> DeletePatientAsync(string email, string password);
        Task<BookAppointmentDto> BookAppointmentAsync(BookAppointmentDto appointmentDetails);

        Task<GetAppointmentDetailsDto> GetAppointmentsByUserAsync(string appointmentId);

        Task<List<SearchDoctorByPatientDto>> SearchDoctorAsync(string name= null, string specialization = null);

        Task<GetPrescriptionDto> GetPrescriptionAsync(string appointmentId);
    }
}

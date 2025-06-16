using Hospital.Dto.Application;
using Hospital.Dto.Application.Doctor;
using Hospital.Dto.Application.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Application.Doctor
{
    public interface IDoctorService
    {
        Task<AddDoctorDetailsDto> AddDoctorDetailsAsync(AddDoctorDetailsDto doctorDetails);
        Task<DoctorDetailsDto> UpdateDoctorDetailsAsync(DoctorDetailsDto doctorDetails);
        Task<DoctorDetailsDto> GetDoctorDetailsByUserIdAsync();
        Task<bool> DeleteDoctorAsync(string email, string password);
        Task<BookAppoinmentUpdateDto> UpdateAppointmentBYDoctorAsync(BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId);
        Task<WritePrescriptionDto> WritePrescriptionAsync(WritePrescriptionDto writePrescriptionDto, string appointmentId);

        Task<List<MedicineDto>> AddMedicinePrescriptionAsync(List<MedicineDto> medicineDto ,string appointmentId);

        Task<List<GetAppointmentDetailsDto>> GetAllAppointmentDetailsAsync();
    }
}

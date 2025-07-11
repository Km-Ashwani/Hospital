﻿using Hospital.Dto.Application;
using Hospital.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Application.Admin
{
    public interface IAdminService
    {
        Task<string> RegisterHospitalMember(RegisterDto registerDto);
        Task<List<DoctorDetailsDto>> GetAllDoctorDetailsAsync();
        Task<List<NurseDetailsDto>> GetAllNurseDetailsAsync();
        Task<List<PatientsDetailsDto>> GetAllPatientDetailsAsync();
        Task<BookAppoinmentUpdateDto> UpdateAppointmentByAdminAsync(BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId, string doctorId);

    }
}

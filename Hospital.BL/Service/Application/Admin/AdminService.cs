using AutoMapper;
using Hospital.BL.Interface.Application.Admin;
using Hospital.Db.AppLicationDbContext;
using Hospital.Dto.Application;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Service.Application.Admin
{
    public class AdminService:IAdminService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminService(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<DoctorDetailsDto>> GetAllDoctorDetailsAsync()
        {
            try
            {
                // Fetch all doctor details from the database, including related AppUser data
                var Role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Doctor");
                if (Role == null)
                {
                    throw new Exception("Role 'Doctor' does not exist in the database.");
                }
                var allDoctorDetails = await _context.DoctorDetails.Include(x => x.AppUser).ToListAsync();
                var allDoctorDetailsDto = _mapper.Map<List<DoctorDetailsDto>>(allDoctorDetails);
                return allDoctorDetailsDto;
            }
            catch (Exception ex)
            {
                throw new Exception("not found", ex);
            }
        }

        public Task<List<NurseDetailsDto>> GetAllNurseDetailsAsync()
        {
            var nurseDetails = _context.NurseDetails
                .Include(n => n.AppUser)
                .Select(n => _mapper.Map<NurseDetailsDto>(n))
                .ToListAsync();
            return nurseDetails;
        }

        public Task<List<PatientsDetailsDto>> GetAllPatientDetailsAsync()
        {
            var patientDetails = _context.PatientDetails
                 .Include(p => p.AppUser)
                 .Select(p => _mapper.Map<PatientsDetailsDto>(p))
                 .ToListAsync();
            return patientDetails;
        }

        public async Task<BookAppoinmentUpdateDto> UpdateAppointmentByAdminAsync(BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId)
        {
            try
            {
                var appointmentIds = Guid.Parse(appointmentId);
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentIds);
                if (appointment == null)
                {
                    throw new Exception("Appointment not found.");
                }
                appointment.AppointmentId = appointmentIds;
                appointment.AppointmentDate = bookAppoinmentUpdateDto.AppointmentDate;
                appointment.Status = bookAppoinmentUpdateDto.Status;

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
                return _mapper.Map<BookAppoinmentUpdateDto>(appointment);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while saving doctor details.", ex);
            }
        }
    }
}

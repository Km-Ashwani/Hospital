using AutoMapper;
using FuzzySharp;
using Hospital.BL.Interface.Application.Email;
using Hospital.BL.Interface.Application.Patient;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using Hospital.Db.Models.Patients;
using Hospital.Dto.Application;
using Hospital.Dto.Application.Labtecnician;
using Hospital.Dto.Application.Patient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Service.Application.Patient
{
    public class PatientService : IPatientService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public PatientService(AppDbContext context, IMapper mapper, UserManager<AppUsers> userManager, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<AddPatientDetailsDto> AddPatientDetailsAsync(AddPatientDetailsDto patientDetails)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var patientId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(patientId))
                {
                    throw new Exception("patientId claim not found");
                }

                if (patientDetails == null)
                    throw new ArgumentNullException(nameof(patientDetails), "Parameter can't be null.");

                var User = await _userManager.FindByIdAsync(patientId);
                if (User == null)
                    throw new Exception("Patient user does not exist.");


                // Update Identity user fields
                User.firstName = patientDetails.FirstName;
                User.lastName = patientDetails.LastName;
                User.AdharNo = patientDetails.AdharNo;
                User.Address = patientDetails.Address;
                User.Gender = patientDetails.Gender;
                User.DateOfBirth = patientDetails.DateOfBirth;

                var identityResult = await _userManager.UpdateAsync(User);
                if (!identityResult.Succeeded)
                {
                    var errorMessage = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update IdentityUser: {errorMessage}");
                }

                var role = await _userManager.GetRolesAsync(User);
                if (role == null || !role.Contains("Patient"))
                    throw new Exception("User is not assigned the Patient role.");

                var patientEntity = _mapper.Map<PatientDetails>(patientDetails);
                patientEntity.UserId = User.Id; // Ensure the UserId is set 

                await _context.PatientDetails.AddAsync(patientEntity);
                await _context.SaveChangesAsync();

                return _mapper.Map<AddPatientDetailsDto>(patientEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while saving doctor details.", ex.InnerException);
            }
        }

        public async Task<bool> DeletePatientAsync(string email, string password)
        {
            var Patient = await _context.PatientDetails.Include(d => d.AppUser).FirstOrDefaultAsync(i => i.AppUser.Email == email);
            if (Patient == null)
            {
                throw new Exception("Doctor not found.");
            }
            var isValidpassword = await _userManager.CheckPasswordAsync(Patient.AppUser, password);
            if (!isValidpassword)
            {
                throw new Exception("Invalid password.");
            }
            var identityResult = await _userManager.DeleteAsync(Patient.AppUser);
            if (!identityResult.Succeeded)
            {
                throw new Exception("Failed to delete AppUser: " + string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            }
            _context.PatientDetails.Remove(Patient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BookAppointmentDto> BookAppointmentAsync(BookAppointmentDto appointmentDetails)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("DoctorId claim not found");
                }
                if (appointmentDetails == null)
                    throw new ArgumentNullException(nameof(appointmentDetails), "Parameter can't be null.");

                var Doctor = await _context.DoctorDetails.Include(x => x.AppUser)
                    .FirstOrDefaultAsync(x => x.UserId == appointmentDetails.DoctorId);
                if (Doctor == null)
                    throw new Exception("Doctor not found.");
                var Patient = await _context.PatientDetails.Include(x => x.AppUser)
                    .FirstOrDefaultAsync(x => x.UserId == userId);
                if (Patient == null)
                    throw new Exception("Patient not found Please register first.");

                var appointmentExists = await _context.Appointments
                    .AnyAsync(a => a.DoctorId == Doctor.AppUser.Id && a.PatientId == Patient.AppUser.Id && a.AppointmentDate == appointmentDetails.AppointmentDate);

                if (appointmentExists)
                {
                    throw new Exception("Appointment already exists for this doctor and patient on the specified date.");
                }

                var adminRole = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "Admin");
                if (adminRole == null)
                    throw new Exception("Admin role not found.");
                var adminUserId = await _context.UserRoles
                                .Where(ur => ur.RoleId == adminRole.Id)
                                .Select(ur => ur.UserId)
                                .FirstOrDefaultAsync();

                if (adminUserId == null)
                    throw new Exception("No user found with Admin role.");

                var adminUser = await _context.Users
                    .Where(u => u.Id == adminUserId)
                    .FirstOrDefaultAsync();

                if (adminUser == null || string.IsNullOrEmpty(adminUser.Email))
                {
                    throw new Exception("Admin email not found.");
                }

                var appointmentEntity = _mapper.Map<Appointments>(appointmentDetails);

                appointmentEntity.PatientId = userId;



                await _context.Appointments.AddAsync(appointmentEntity);
                await _context.SaveChangesAsync();

                var appointmentId = _context.Appointments
                    .Where(a => a.DoctorId == Doctor.AppUser.Id && a.PatientId == Patient.AppUser.Id && a.AppointmentDate == appointmentDetails.AppointmentDate)
                    .Select(a => a.AppointmentId)
                    .FirstOrDefault();

                string adminEmail = adminUser.Email;
                string subject = "New Appointment Notification";
                string body = $"Dear ,{adminUser.firstName}-{adminUser.lastName}<br/><br/>" +
                              $"A new appointment has been booked.<br/>" +
                              $"<Strong>Doctor Id- {Doctor.AppUser.Id}<Strong/> Name :{Doctor.AppUser.firstName} {Doctor.AppUser.lastName}<br/>" +
                              $"<Strong>Patient Id- {Patient.AppUser.Id}<Strong/> Name :{Patient.AppUser.firstName} {Patient.AppUser.lastName}<br/>" +
                              $"<strong>Appointment Id:</strong> {appointmentId}<br/><br/>" +
                              $"<strong>Reason:</strong> {appointmentDetails.Reason}<br/><br/>" +
                              $"<strong>Date:</strong> {appointmentDetails.AppointmentDate}<br/><br/>" +
                              $"Regards,<br/>Hospital Management System";

                await _emailService.SendEmailAsync(adminEmail, subject, body);

                return _mapper.Map<BookAppointmentDto>(appointmentEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Wrong", ex);
            }

        }
        public async Task<GetAppointmentDetailsDto> GetAppointmentsByUserAsync(string appointmentId)
        {
            try
            {
                var id = Guid.Parse(appointmentId);
                var appointments = await _context.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == id);
                if (appointments == null)
                {
                    throw new Exception("No appointments found for the patient.");
                }
                return _mapper.Map<GetAppointmentDetailsDto>(appointments);
            }
            catch (Exception ex)
            {
                throw new Exception("No appointment found", ex.InnerException);
            }
        }

        public async Task<PatientsDetailsDto> UpdatePatientDetailsAsync(PatientsDetailsDto patientDetails)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var patientId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(patientId))
                {
                    throw new Exception("PatientId claim not found");
                }

                var patient = _mapper.Map<PatientDetails>(patientDetails);

                if (patient.UserId != patientId)
                {
                    throw new Exception("Access denied");
                }
                if (patient == null)
                {
                    throw new ArgumentNullException(nameof(patientDetails), "Parameter can't be null.");
                }
                _context.PatientDetails.Update(patient);
                await _context.SaveChangesAsync();
                return _mapper.Map<PatientsDetailsDto>(patient);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while updating patient details.", ex.InnerException);
            }
        }

        public async Task<PatientsDetailsDto> GetPatientDetailsByUserIdAsync()
        {
            try
            {

                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("DoctorId claim not found");
                }
                var patient = await _context.PatientDetails.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.UserId == userId);

                if (patient == null)
                {
                    throw new Exception("Patient not found.");
                }
                return _mapper.Map<PatientsDetailsDto>(patient);
            }
            catch (Exception ex)
            {
                throw new Exception("error while getting patient details", ex);
            }
        }

        public async Task<List<SearchDoctorByPatientDto>> SearchDoctorAsync(string? name = null, string? specialization = null)
        {
            try
            {
                var doctors = await _context.DoctorDetails
                .Include(x => x.AppUser)
                .Where(x => x.IsAvailable == true)
                .ToListAsync();

                var matchedDoctors = doctors.Where(x =>
                    (string.IsNullOrEmpty(name) ||
                         (!string.IsNullOrEmpty(x.AppUser?.firstName) &&
                        (x.AppUser.firstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                        Fuzz.PartialRatio(x.AppUser.firstName.ToLower(), name.ToLower()) > 80)) ||
                        (!string.IsNullOrEmpty(x.AppUser?.lastName) &&
                        (x.AppUser.lastName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                        Fuzz.PartialRatio(x.AppUser.lastName.ToLower(), name.ToLower()) > 80)))
                         &&
                        (string.IsNullOrEmpty(specialization) ||
                        x.Specialization.Contains(specialization, StringComparison.OrdinalIgnoreCase) ||
                        Fuzz.PartialRatio(x.Specialization.ToLower(), specialization.ToLower()) > 80)
                ).ToList();

                if (!matchedDoctors.Any())
                    throw new Exception("No matching doctor found.");

                return _mapper.Map<List<SearchDoctorByPatientDto>>(matchedDoctors);
            }
            catch (Exception ex)
            {
                throw new Exception("Doctor not found", ex.InnerException);
            }
        }

        public async Task<GetPrescriptionDto> GetPrescriptionAsync(string appointmentId)
        {

            try
            {
                var prescription = await _context.Prescription.Include(m => m.Medicines).FirstOrDefaultAsync(p => p.AppointmentId == Guid.Parse(appointmentId));
                if (prescription == null)
                {
                    throw new Exception("Prescription  found but appointment not completed.");
                }

                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == prescription.AppointmentId);
                if (appointment == null || appointment.Status.ToString() != "Completed")
                {
                    throw new Exception("Appointment not found for the given prescription or appointment  not completed");
                }
                return _mapper.Map<GetPrescriptionDto>(prescription);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while retrieving prescription.", ex.InnerException);

            }
        }

        public async Task<LabTestDto> GetLabTestAsync(string appointmentId)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Labtechnician)
                .FirstOrDefaultAsync(a => a.AppointmentId == Guid.Parse(appointmentId));
            if (appointment == null)
            {
                throw new Exception("Appointment not found.");
            }
            var prescription = await _context.Prescription
                .Where(p => p.AppointmentId == Guid.Parse(appointmentId))
                .FirstOrDefaultAsync();
            if (prescription == null)
            {
                throw new Exception("Prescription not found for this appointment.");
            }

            var labTest = await _context.LabTests
                .Where(l => l.AppointmentId == Guid.Parse(appointmentId))
                .FirstOrDefaultAsync();
            return _mapper.Map<LabTestDto>(labTest);
        }
    }
}

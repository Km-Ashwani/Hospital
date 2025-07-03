using AutoMapper;
using Hospital.BL.Interface.Application.Admin;
using Hospital.BL.Interface.Application.Email;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Dto.Application;
using Hospital.Dto.Auth;
using Microsoft.AspNetCore.Identity;
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
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUsers> _userManager;

        public AdminService(AppDbContext context,IMapper mapper,IEmailService emailService, RoleManager<IdentityRole> roleManager, UserManager<AppUsers> userManager)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<string> RegisterHospitalMember(RegisterDto registerDto)
        {
            try
            {
                if (registerDto == null)
                {
                    throw new ArgumentNullException(nameof(registerDto), "RegisterDto cannot be null");
                }
                var user = new AppUsers
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    PhoneNumber = registerDto.Phone,
                };
                var result = await _context.AppUsers.FirstOrDefaultAsync(e => e.Email.ToLower() == registerDto.Email.ToLower());
                if (result != null)
                {
                    throw new Exception("user already Exits");
                }
                var createResult = await _userManager.CreateAsync(user, registerDto.Password);
                if (!createResult.Succeeded)
                {
                    var errorMessages = string.Join(", ", createResult.Errors.Select(e => e.Description));
                    throw new Exception("User creation failed: " + errorMessages);
                }

                string userEmail = registerDto.Email;
                string subject = "Registration Successful";
                string body = $@"Dear {registerDto.Email},
                              We are delighted to inform you that your registration was successfully completed. 
                              You have been assigned the role of <strong>{registerDto.Password}</strong> in our system.
                              Thank you for choosing to register with <strong>Hospital Management System</strong>. 
                              We look forward to supporting you.

                              If you have any questions or need assistance, please feel free to contact our support team.

                              Best regards,  
                              <strong>Hospital Team</strong>";

                if (string.IsNullOrEmpty(registerDto.Role))
                {
                    throw new Exception("Role cannot be null or empty");
                }


                await _emailService.SendEmailAsync(userEmail, subject, body);

                await AssignRoleAsync(registerDto.Email, registerDto.Role);
                return "User created successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong: " + ex.Message + " | " + ex.InnerException?.Message);
            }

        }


        private async Task<bool> AssignRoleAsync(string email, string roleName)
        {
            try
            {
                var user = await _context.AppUsers.FirstOrDefaultAsync(e => (e.Email ?? "").ToLower() == email.ToLower());
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!result.Succeeded)
                    {
                        throw new Exception("Failed to create role: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            catch (Exception)
            {
                throw new Exception();
            }
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

        public async Task<BookAppoinmentUpdateDto> UpdateAppointmentByAdminAsync(BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId, string doctorId)
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

                var doctor = await _context.DoctorDetails
                    .Include(d => d.AppUser)
                    .FirstOrDefaultAsync(d => d.AppUser.Id == doctorId);
                if (doctor == null)
                {
                    throw new Exception("Doctor not found");
                }

                var patient = await _context.PatientDetails
                    .Include(p => p.AppUser)
                    .FirstOrDefaultAsync(p => p.AppUser.Id == bookAppoinmentUpdateDto.PatientId);
                if (patient == null)
                    throw new Exception("Patient not found");

                string doctorEmail = doctor.AppUser.Email;
                string subject = "Appointment Updated";
                string body = $"Dear {doctor.AppUser.firstName}-{doctor.AppUser.lastName},<br/><br/>" +
                              $"A new appointment has been booked.<br/>" +
                              $"<Strong>Doctor Id {doctor.AppUser.Id}<Strong/> Name :{doctor.AppUser.firstName} {doctor.AppUser.lastName}<br/>" +
                              $"<Strong>Patient Id {patient.AppUser.Id}<Strong/> Name :{patient.AppUser.firstName} {patient.AppUser.lastName}<br/>" +
                               $"<strong>Status :</strong> {appointment.AppointmentId}<br/><br/>" +
                              $"<strong>Status :</strong> {bookAppoinmentUpdateDto.Status}<br/><br/>" +
                              $"<strong>Date:</strong> {bookAppoinmentUpdateDto.AppointmentDate}<br/><br/>" +
                              $"Regards,<br/>Hospital Management System";



                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
                await _emailService.SendEmailAsync(doctorEmail, subject, body);
                return _mapper.Map<BookAppoinmentUpdateDto>(appointment);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong .", ex);
            }
        }
    }
}

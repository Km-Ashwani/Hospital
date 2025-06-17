using AutoMapper;
using Hospital.BL.Interface.Application.Doctor;
using Hospital.BL.Interface.Application.Email;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using Hospital.Db.Models.Doctor;
using Hospital.Dto.Application;
using Hospital.Dto.Application.Doctor;
using Hospital.Dto.Application.Patient;
using Hospital.Dto.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Service.Application.Doctor
{
    public class DoctorService : IDoctorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public DoctorService(AppDbContext context, IMapper mapper, UserManager<AppUsers> userManager, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        public async Task<AddDoctorDetailsDto> AddDoctorDetailsAsync(AddDoctorDetailsDto doctorDetails)
        {
            try
            {

                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var doctorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(doctorId))
                {
                    throw new Exception("DoctorId claim not found");
                }
                if (doctorDetails == null)
                    throw new ArgumentNullException(nameof(doctorDetails), "Parameter can't be null.");

                var User = await _userManager.FindByIdAsync(doctorId);
                if (User == null)
                    throw new Exception("Doctor user does not exist.");

                var existingDoctor = await _context.DoctorDetails
                                          .FirstOrDefaultAsync(d => d.UserId == User.Id);
                if (existingDoctor != null)
                {
                    throw new Exception("Doctor details already exist for this user.");
                }

                User.firstName = doctorDetails.FirstName;
                User.lastName = doctorDetails.LastName;
                User.AdharNo = doctorDetails.AdharNO;
                User.Address = doctorDetails.Address;
                User.DateOfBirth = doctorDetails.DateOfBirth;
                User.Gender = doctorDetails.Gender;

                var identityResult = await _userManager.UpdateAsync(User);
                if (!identityResult.Succeeded)
                {
                    var errorMessage = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update IdentityUser: {errorMessage}");
                }
                var role = await _userManager.GetRolesAsync(User);
                if (!role.Contains("Doctor"))
                {
                    await _userManager.AddToRoleAsync(User, "Doctor");
                    //throw new Exception("User is not assigned the Doctor role.");
                }


                var doctorEntity = _mapper.Map<DoctorDetails>(doctorDetails);

                doctorEntity.UserId = User.Id;  // Ensure UserId is set
                await _context.DoctorDetails.AddAsync(doctorEntity);
                await _context.SaveChangesAsync();

                return _mapper.Map<AddDoctorDetailsDto>(doctorEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while saving doctor details.", ex);
            }
        }



        public async Task<bool> DeleteDoctorAsync(string email, string password)
        {
            try
            {
                var doctor = await _context.DoctorDetails.Include(d => d.AppUser).FirstOrDefaultAsync(i => i.AppUser.Email == email);
                if (doctor == null)
                {
                    throw new Exception("Doctor not found.");
                }
                var isPasswordValid = await _userManager.CheckPasswordAsync(doctor.AppUser, password);
                if (!isPasswordValid)
                {
                    throw new Exception("Invalid password. Account deletion failed.");
                }
                var identityResult = await _userManager.DeleteAsync(doctor.AppUser);
                if (!identityResult.Succeeded)
                {
                    throw new Exception("Failed to delete AppUser: " + string.Join(", ", identityResult.Errors.Select(e => e.Description)));
                }
                _context.DoctorDetails.Remove(doctor);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var detailedMessage = $"Error: {ex.Message}";
                if (ex.InnerException != null)
                {
                    detailedMessage += $" | Inner: {ex.InnerException.Message}";
                }

                throw new Exception(detailedMessage);

            }
        }

        public async Task<DoctorDetailsDto> GetDoctorDetailsByUserIdAsync()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var doctorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(doctorId))
                {
                    throw new Exception("DoctorId claim not found");
                }
                var doctorDetails = await _context.DoctorDetails
               .Include(d => d.AppUser)
               .FirstOrDefaultAsync(d => d.UserId == doctorId);
                if (doctorDetails == null)
                {
                    throw new Exception("Doctor not found.");
                }

                var doctorDetailsDto = _mapper.Map<DoctorDetailsDto>(doctorDetails);
                return doctorDetailsDto;
            }
            catch (Exception ex)
            {

                throw new Exception("not found", ex);
            }
        }


        public async Task<BookAppoinmentUpdateDto> UpdateAppointmentBYDoctorAsync(BookAppoinmentUpdateDto bookAppoinmentUpdateDto, string appointmentId)
        {
            try
            {
                var appointmentIds = Guid.Parse(appointmentId);
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentIds);
                if (appointment == null)
                {
                    throw new Exception("Appointment not found.");
                }
                if (appointment.Status == null || appointment.Status.ToString() != "Pending")
                {
                    throw new Exception("Wait for Admin response");
                }
                if (appointment.DoctorId != bookAppoinmentUpdateDto.DoctorId)
                {
                    throw new Exception("You haven't book appointment for this doctor");
                }
                var patient = await _context.PatientDetails
                   .Include(p => p.AppUser)
                   .FirstOrDefaultAsync(p => p.UserId == bookAppoinmentUpdateDto.PatientId);
                if (appointment.AppointmentDate != bookAppoinmentUpdateDto.AppointmentDate)
                {
                    throw new Exception("You can't change appointment date, please contact admin for this issue.");
                }

                var doctor = await _context.DoctorDetails
                    .Include(d => d.AppUser)
                    .FirstOrDefaultAsync(d => d.UserId == bookAppoinmentUpdateDto.DoctorId);

                appointment.AppointmentId = appointmentIds;
                appointment.AppointmentDate = bookAppoinmentUpdateDto.AppointmentDate;
                appointment.Status = bookAppoinmentUpdateDto.Status;

                string patientEmail = patient?.AppUser?.Email;
                string subject = "Appointment Update Notification";
                string body = $@"<p>Dear <strong>{patient.AppUser.firstName} {patient.AppUser.lastName}</strong>,</p>
                                 <p>We are pleased to inform you that your appointment request has been 
                                 <strong style='color:green;'>successfully confirmed</strong>.</p>

                                 <p><strong>Appointment Details:</strong></p>
                                 <ul>
                                    <li><strong>Doctor ID:</strong> {appointment.DoctorId}</li>
                                    <li><strong>Doctor Name:</strong> Dr. {doctor.AppUser.firstName} {doctor.AppUser.lastName}</li>
                                    <li><strong>Appointment Date:</strong> {appointment.AppointmentDate:dd MMM yyyy}</li>
                                 </ul>
                                 <p style='margin-top:10px;'>To proceed further, we kindly request you to 
                                 <strong>complete the payment</strong> at the earliest convenience. Please note that the consultation with your doctor will only be valid after the payment has been successfully processed.</p>
                                 <p>After your payment is verified, you will receive:</p>
                                 <ul>
                                    <li>A payment confirmation email</li>
                                    <li>Access to full appointment details</li>
                                    <li>A unique appointment code to present at the hospital/reception</li>
                                 </ul>
                                 <p style='margin-top:10px;'>
                                    If you have any questions or need assistance regarding your appointment or payment process, feel free to contact our support team.
                                 </p>
                                 <p><strong>Note:</strong> Please make sure to carry your appointment code and valid ID proof when you visit the hospital.</p><br />
                                 <p>Thank you for choosing <strong>Hospital Management System</strong>. We are committed to providing you with the best possible care.</p>
                                 <p>We wish you good health and look forward to serving you!</p>";


                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(patientEmail))
                {
                    await _emailService.SendEmailAsync(patientEmail, subject, body);
                }
                else
                {
                    throw new Exception("Patient email not found.");
                }
                return _mapper.Map<BookAppoinmentUpdateDto>(appointment);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while saving doctor details.", ex.InnerException);
            }
        }

        public Task<DoctorDetailsDto> UpdateDoctorDetailsAsync(DoctorDetailsDto doctorDetails)
        {
            try
            {
                if (doctorDetails == null)
                    throw new ArgumentNullException(nameof(doctorDetails), "Parameter can't be null.");
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var doctorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(doctorId))
                {
                    throw new Exception("DoctorId claim not found");
                }
                var doctorEntity = _mapper.Map<DoctorDetails>(doctorDetails);
                if (doctorEntity.UserId != doctorId)
                {
                    throw new Exception("Doctor details do not belong to the current user.");
                }
                if (doctorEntity == null)
                    throw new ArgumentNullException(nameof(doctorDetails), "doctor not found");

                _context.DoctorDetails.Update(doctorEntity);
                _context.SaveChangesAsync();
                return Task.FromResult(_mapper.Map<DoctorDetailsDto>(doctorEntity));
            }
            catch (Exception)
            {
                throw new Exception("something went wrong whi update doctorDetails");
            }
        }

        public async Task<WritePrescriptionDto> WritePrescriptionAsync(WritePrescriptionDto writePrescriptionDto, string appointmentId)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var doctorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(doctorId))
                {
                    throw new Exception("DoctorId claim not found");
                }

                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == Guid.Parse(appointmentId));
                if (appointment == null || appointment.DoctorId != doctorId)
                {
                    throw new Exception("Appointment not found or you haven't book appointment for this doctor");
                }
                var payment = await _context.AppointmentPayment.FirstOrDefaultAsync(p => p.AppointmentId == appointment.AppointmentId);
                if (payment == null)
                {
                    throw new Exception("payment not found");
                }
                if (payment.Status.ToString() != "Success")
                {
                    throw new Exception("you have not pay for prescription  yet");
                }

                var prescription = _mapper.Map<Prescription>(writePrescriptionDto);
                prescription.AppointmentId = appointment.AppointmentId; // Ensure AppointmentId is set

                await _context.Prescription.AddAsync(prescription);
                await _context.SaveChangesAsync();

                var prescriptionDto = _mapper.Map<WritePrescriptionDto>(prescription);
                return prescriptionDto;
            }
            catch (Exception ex)
            {
                throw new Exception("something went wrong", ex);
            }
        }

        public async Task<List<MedicineDto>> AddMedicinePrescriptionAsync(List<MedicineDto> medicineDto, string PrescriptionId)
        {
            try
            {
                var appointment = await _context.Prescription.FirstOrDefaultAsync(a => a.PrescriptionId == Guid.Parse(PrescriptionId));
                if (appointment == null)
                {
                    throw new Exception("Prescription not found.");
                }
                var medicineEntity = _mapper.Map<List<PrescriptionMedicine>>(medicineDto);
                foreach (var medicine in medicineEntity)
                {
                    medicine.PrescriptionId = appointment.PrescriptionId;// Ensure PrescriptionId is set
                }
                await _context.PrescriptionMedicine.AddRangeAsync(medicineEntity);
                await _context.SaveChangesAsync();
                return _mapper.Map<List<MedicineDto>>(medicineEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Something Wrong", ex);
            }
        }

        public async Task<List<GetAppointmentDetailsDto>> GetAllAppointmentDetailsAsync()
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null)
                {
                    throw new Exception("User context not available");
                }

                var doctorId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(doctorId))
                {
                    throw new Exception("DoctorId claim not found");
                }
                var appointments = await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
                return _mapper.Map<List<GetAppointmentDetailsDto>>(appointments);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while fetching appointment details.", ex.InnerException);
            }
        }
    }
}

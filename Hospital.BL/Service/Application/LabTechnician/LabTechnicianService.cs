using AutoMapper;
using Hospital.BL.Interface.Application.Email;
using Hospital.BL.Interface.Application.LabTechnician;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Db.Models.Doctor;
using Hospital.Db.Models.Labtechcian;
using Hospital.Db.Models.LabTests;
using Hospital.Dto.Application.Doctor;
using Hospital.Dto.Application.Labtecnician;
using Hospital.Dto.Application.Patient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Service.Application.LabTechnician
{
    public class LabTechnicianService : ILabTechnicianService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public LabTechnicianService(AppDbContext context,IMapper mapper,UserManager<AppUsers> userManager,IHttpContextAccessor httpContextAccessor,IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }
        public async Task<AddLabTechnicianDto> AddLabTechnicianDto(AddLabTechnicianDto labTechnicianDto)
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
                if (labTechnicianDto == null)
                    throw new ArgumentNullException(nameof(labTechnicianDto), "Parameter can't be null.");

                var technician = await _userManager.FindByIdAsync(userId);
                if (technician == null)
                    throw new Exception("Doctor user does not exist.");

                var existingTechnician = await _context.Labtechnicians
                                          .FirstOrDefaultAsync(d => d.UserId == technician.Id);
                if (existingTechnician != null)
                {
                    throw new Exception("Doctor details already exist for this user.");
                }

                technician.firstName = labTechnicianDto.FirstName;
                technician.lastName = labTechnicianDto.LastName;
                technician.AdharNo = labTechnicianDto.AdharNO;
                technician.Address = labTechnicianDto.Address;
                technician.DateOfBirth = labTechnicianDto.DateOfBirth;
                technician.Gender = labTechnicianDto.Gender;

                var identityResult = await _userManager.UpdateAsync(technician);
                if (!identityResult.Succeeded)
                {
                    var errorMessage = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update IdentityUser: {errorMessage}");
                }
                var role = await _userManager.GetRolesAsync(technician);
                if (!role.Contains("LabTechnician"))
                {
                    await _userManager.AddToRoleAsync(technician, "LabTechnician");
                }


                var technicianEntity = _mapper.Map<Labtechnicians>(labTechnicianDto);

                technicianEntity.UserId = technician.Id;  // Ensure UserId is set
                await _context.Labtechnicians.AddAsync(technicianEntity);
                await _context.SaveChangesAsync();

                return _mapper.Map<AddLabTechnicianDto>(technicianEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while saving LabTechnician details.", ex);
            }
        }

        public async Task<PaymentDto> PaymentAsync(PaymentDto paymentDto, string appointmentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
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


                var appointment = await _context.Appointments.FindAsync(appointmentId);

                if (appointment == null)
                    throw new Exception("Appointment not found");

                if ((appointment.Status).ToString() != "Confirm")
                    throw new Exception(" the appointment is not confirm yet");

                if (paymentDto.Amount <= 0)
                    throw new Exception("Invalid payment amount.");
                var labtechnician = await _userManager.FindByIdAsync(userId);
                if (labtechnician == null)
                    throw new Exception("Receptionist not found.");

                var existingPayment = await _context.LabPayments
                        .FirstOrDefaultAsync(p => p.Status == PaymentStatus.Success);

                if (existingPayment != null)
                    throw new Exception("Payment has already been completed for this appointment.");


                appointment.LabTechnicianId = labtechnician.Id;

                var patientUserId = appointment.PatientId; // Assuming PatientId is the user ID of the patient

                var id = Guid.Parse(appointmentId);
                var payment = new LabPayment
                {
                    AppointmentId = id,
                    Amount = paymentDto.Amount,
                    PaymentMethod = paymentDto.paymentMethod,
                    Status = PaymentStatus.Success,
                    TransactionId = Guid.NewGuid().ToString(),
                    PatientUserId = patientUserId, // Assuming patientUserId is part of PaymentDto
                };


                if (payment.Status != PaymentStatus.Success)
                {
                    throw new Exception("Payment failed. Please try again.");
                }
                await _context.LabPayments.AddAsync(payment);
                await _context.SaveChangesAsync();

                // ✅ Commit only if everything worked
                await transaction.CommitAsync();
                return _mapper.Map<PaymentDto>(payment);
            }
            catch (Exception ex)
            {
                // ❌ Rollback if anything fails
                await transaction.RollbackAsync();
                throw new Exception($"Payment processing failed: {ex.Message}", ex);
            }
        }

        public async Task<LabTestDto> AddLabTestAsync(LabTestDto labTestDto, string appointmentId)
        {
            var appointment =await _context.Appointments
                .Include(a => a.Labtechnician)
                .FirstOrDefaultAsync(a => a.AppointmentId == Guid.Parse(appointmentId));
            if (appointment == null)
            {
                throw new Exception("Appointment not found.");
            }
            if (appointment.Labtechnician == null)
            {
                throw new Exception("Lab Technician not assigned to this appointment.");
            }

            var Prescription = _context.Prescription.Where(p => p.AppointmentId == Guid.Parse(appointmentId)).FirstOrDefault();
            if (Prescription == null) {
                throw new Exception("Prescription not found for this appointment.");
            }
            var labTest = _mapper.Map<LabTest>(labTestDto);
            if (Prescription.IsLabTestRequired == true)
            {
               
                foreach (var item in labTestDto.labTestItemDtos)
                {
                    var labTestItem = _mapper.Map<LabTestItem>(item);
                    labTest.LabTests.Add(labTestItem);
                }

                labTest.AppointmentId = Guid.Parse(appointmentId);
                labTest.PatientUserId = appointment.PatientId;
                labTest.DoctorUserId = appointment.DoctorId;
                labTest.Appointment = appointment;
                    
                await _context.LabTests.AddAsync(labTest);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Lab Test not prescribed in the appointment.");
            }

            return _mapper.Map<LabTestDto>(labTest);
        }
    }
}

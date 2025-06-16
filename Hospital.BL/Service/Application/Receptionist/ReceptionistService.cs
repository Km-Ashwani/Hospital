using AutoMapper;
using Hospital.BL.Interface.Application.Receptionist;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using Hospital.Db.Models.Receptionist;
using Hospital.Dto.Application.Patient;
using Hospital.Dto.Application.Receptionist;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.BL.Service.Application.Receptionist
{
    public class ReceptionistService:IReceptionistService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;

        public ReceptionistService(AppDbContext context, IMapper mapper, UserManager<AppUsers> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<AddReceptionistDetailsDto> AddReceptionistDetailAsync(AddReceptionistDetailsDto addReceptionistDetailsDto ,string email)
        {
            try
            {
                if (addReceptionistDetailsDto == null)
                    throw new ArgumentNullException(nameof(addReceptionistDetailsDto), "Parameter can't be null.");

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    throw new Exception("Receptionist user does not exist.");

                // Update Identity user fields
                user.firstName = addReceptionistDetailsDto.FirstName;
                user.lastName = addReceptionistDetailsDto.LastName;
                user.AdharNo = addReceptionistDetailsDto.AdharNo;
                user.Address = addReceptionistDetailsDto.Address;
                user.Gender = addReceptionistDetailsDto.Gender;
                user.DateOfBirth = addReceptionistDetailsDto.DateOfBirth;

                var identityResult = await _userManager.UpdateAsync(user);
                if (!identityResult.Succeeded)
                {
                    var errorMessage = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update IdentityUser: {errorMessage}");
                }

                // Role check
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || !roles.Contains("Receptionist"))
                    throw new Exception("User is not assigned the Receptionist role.");

                // Map and save addReceptionistDetailsDto
                var receptionistEntity = _mapper.Map<ReceptionistDetails>(addReceptionistDetailsDto);
                receptionistEntity.UserId = user.Id; // Ensure UserId is set
                await _context.receptionistDetails.AddAsync(receptionistEntity);
                await _context.SaveChangesAsync();

                return _mapper.Map<AddReceptionistDetailsDto>(receptionistEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while saving Receptionist details: " + ex.Message);
            }
        }

        public async Task<PaymentDto> PaymentAsync(PaymentDto paymentDto, string patientUserId,string receptionistId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var appointment = await _context.Appointments.FindAsync(paymentDto.appointmentId);
               
                if (appointment == null)
                    throw new Exception("Appointment not found");

                if ((appointment.Status).ToString() != "Confirm")
                    throw new Exception(" the appointment is not confirm yet");

                if (paymentDto.Amount <= 0)
                    throw new Exception("Invalid payment amount.");
                var receptionist = await _userManager.FindByIdAsync(receptionistId);
                if (receptionist == null)
                    throw new Exception("Receptionist not found.");

                var existingPayment = await _context.AppointmentPayment
                        .FirstOrDefaultAsync(p => p.Status == PaymentStatus.Success);

                if (existingPayment != null)
                    throw new Exception("Payment has already been completed for this appointment.");


                appointment.ReceptionistId = receptionist.Id;

                //_context.Appointments.Update(appointment);

                var payment = new AppointmentPayment
                {
                    AppointmentId = paymentDto.appointmentId,
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
                await _context.AppointmentPayment.AddAsync(payment);
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
    }
}

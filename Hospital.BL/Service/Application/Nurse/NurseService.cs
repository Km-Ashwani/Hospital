using AutoMapper;
using Hospital.BL.Interface.Application.Nurse;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Db.Models.Nurse;
using Hospital.Dto.Application;
using Hospital.Dto.Application.Nurse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Service.Application.Nurse
{
    public class NurseService: INurseService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NurseService(AppDbContext context, IMapper mapper, UserManager<AppUsers> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AddNurseDetailsDto> AddNurseDetailsAsync(AddNurseDetailsDto nurseDetails)
        {
            try
            {
                var users = _httpContextAccessor.HttpContext?.User;

                if (users == null)
                {
                    throw new Exception("User context not available");
                }

                var nurseId = users.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(nurseId))
                {
                    throw new Exception("nurseId claim not found");
                }
                if (nurseDetails == null)
                    throw new ArgumentNullException(nameof(nurseDetails), "Parameter can't be null.");

                var user = await _userManager.FindByIdAsync(nurseId);
                if (user == null)
                    throw new Exception("Nurse user does not exist.");

                // Update Identity user fields
                user.firstName = nurseDetails.FirstName;
                user.lastName = nurseDetails.LastName;
                user.AdharNo = nurseDetails.AdharNo;
                user.Address = nurseDetails.Address;
                user.Gender = nurseDetails.Gender;
                user.DateOfBirth = nurseDetails.DateOfBirth;

                var identityResult = await _userManager.UpdateAsync(user);
                if (!identityResult.Succeeded)
                {
                    var errorMessage = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update IdentityUser: {errorMessage}");
                }

                // Role check
                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || !roles.Contains("Nurse"))
                    throw new Exception("User is not assigned the Nurse role.");

                // Map and save NurseDetails
                var nurseEntity = _mapper.Map<NurseDetails>(nurseDetails);
                nurseEntity.UserId = user.Id; // Ensure UserId is set
                await _context.NurseDetails.AddAsync(nurseEntity);
                await _context.SaveChangesAsync();

                return _mapper.Map<AddNurseDetailsDto>(nurseEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong while saving Nurse details: " + ex.Message);
            }
        }

        public async Task<bool> DeleteNurseAsync( string email, string password)
        {
            var nurse = await _context.NurseDetails.Include(d => d.AppUser).FirstOrDefaultAsync(i => i.AppUser.Email == email);
            if (nurse == null)
            {
                throw new Exception("nurse not found.");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(nurse.AppUser, password);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid password. Account deletion failed.");
            }
            var identityResult = await _userManager.DeleteAsync(nurse.AppUser);
            if (!identityResult.Succeeded)
            {
                throw new Exception("Failed to delete AppUser: " + string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            }
            _context.NurseDetails.Remove(nurse);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<NurseDetailsDto> GetNurseDetailsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<NurseDetailsDto> UpdateNurseDetailsAsync(NurseDetailsDto nurseDetails)
        {
            throw new NotImplementedException();
        }
    }
}

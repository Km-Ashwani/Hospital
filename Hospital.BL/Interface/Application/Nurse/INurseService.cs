using Hospital.Dto.Application;
using Hospital.Dto.Application.Nurse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Application.Nurse
{
    public interface INurseService
    {
        Task<AddNurseDetailsDto> AddNurseDetailsAsync(AddNurseDetailsDto nurseDetails);
        Task<NurseDetailsDto> UpdateNurseDetailsAsync(NurseDetailsDto nurseDetails);
        Task<NurseDetailsDto> GetNurseDetailsByUserIdAsync(string userId);
        Task<bool> DeleteNurseAsync( string email, string password);
    }
}

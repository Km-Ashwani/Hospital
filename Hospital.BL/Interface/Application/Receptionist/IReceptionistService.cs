using Hospital.Dto.Application.Patient;
using Hospital.Dto.Application.Receptionist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Interface.Application.Receptionist
{
    public interface IReceptionistService
    {
        Task<AddReceptionistDetailsDto> AddReceptionistDetailAsync(AddReceptionistDetailsDto addReceptionistDetailsDto);
        Task<PaymentDto> PaymentAsync(PaymentDto paymentDto,string appointmentId);

    }
}

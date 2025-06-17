using Hospital.Db.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Dto.Application.Patient
{
    public class PaymentDto
    {
        public string paymentMethod { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus status { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}

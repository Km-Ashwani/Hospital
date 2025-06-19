using Hospital.Db.Models.Appointment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.Models.LabTests
{
    public class LabPayment
    {
        public Guid labPaymentId { get; set; }

        public Guid AppointmentId { get; set; }
        [ForeignKey("AppointmentId")]
        public LabTest? LabTest { get; set; }

        public string? PatientUserId { get; set; }
        [ForeignKey("PatientUserId")]
        public AppUsers? Patient { get; set; }

        public decimal Amount { get; set; }            // e.g. 500.00
        public string? PaymentMethod { get; set; }      // e.g. "UPI", "CreditCard"
        public PaymentStatus Status { get; set; }      // Enum: Pending, Success, Failed

        public string? TransactionId { get; set; }     // Razorpay / Paytm ID etc.
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}

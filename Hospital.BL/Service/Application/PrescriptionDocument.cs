using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.Service.Application
{
    public class PrescriptionDocument : IDocument
    {
        private readonly Prescription _prescription;

        public PrescriptionDocument(Prescription prescription)
        {
            _prescription = prescription;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content()
                    .Column(col =>
                    {
                        var patient = _prescription.Appointment?.Patient;
                        var doctor = _prescription.Appointment?.Doctor;

                        if (patient == null || doctor == null)
                        {
                            col.Item().Text("Prescription details are incomplete.");
                            return;
                        }

                        // 🏥 Hospital Header
                        col.Item().Row(row =>
                        {
                            row.RelativeColumn().Text("🌐  Hospital Management System")
                                .FontSize(24)
                                .Bold()
                                .FontColor(Colors.Blue.Medium);

                            row.ConstantColumn(50);
                        });

                        col.Item().PaddingBottom(10).Element(container =>
                        {
                            container
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Medium);
                        });


                        // 📄 Prescription Header
                        col.Item()
                            .PaddingBottom(15)
                            .Text("Prescription")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Black);

                        // 👤 Patient & Doctor Info
                        col.Item().Row(r =>
                        {
                            r.RelativeColumn().Text($"👤 Patient Name: {patient.firstName} {patient.lastName}");
                            r.RelativeColumn().Text($"🩺 Doctor Name: {doctor.firstName} {doctor.lastName}");
                        });

                        col.Item().PaddingBottom(10).Text($"🕒 Date: {_prescription.CreatedAt:dd MMM yyyy}")
                            .Italic()
                            .FontColor(Colors.Grey.Darken2);


                        // 💊 Medicines
                        col.Item().PaddingBottom(5)
                            .Text("💊 Medicines:")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Green.Darken2);

                        if (_prescription.Medicines != null && _prescription.Medicines.Any())
                        {
                            foreach (var med in _prescription.Medicines)
                            {
                                col.Item().PaddingLeft(10).Text(
                                    $"• {med?.MedicineName ?? "Unknown"} | {med?.Dosage ?? "?"} | {med?.DurationInDays ?? 0} days | {med?.Frequency ?? "?"}"
                                );
                            }
                        }
                        else
                        {
                            col.Item().Text("No medicines prescribed.");
                        }

                    });

                page.Footer()
                        .AlignCenter()
                        .PaddingTop(10)
                        .PaddingBottom(20)
                        .Column(footer =>
                        {
                            footer.Item().Text("📞 For assistance, contact: +91-9026945023")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                            footer.Item().Text("🧾 Thank you for choosing Hospital Management System")
                            .FontSize(10)
                            .Italic()
                            .FontColor(Colors.Grey.Darken1);
                        });
            });
        }
    }
}

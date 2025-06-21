using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using Hospital.Db.Models.Doctor;
using Hospital.Db.Models.Labtechcian;
using Hospital.Db.Models.LabTests;
using Hospital.Db.Models.Nurse;
using Hospital.Db.Models.Patients;
using Hospital.Db.Models.Receptionist;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Db.AppLicationDbContext
{
    public class AppDbContext : IdentityDbContext<AppUsers>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppUsers> AppUsers { get; set; }
        public DbSet<NurseDetails> NurseDetails { get; set; }
        public DbSet<DoctorDetails> DoctorDetails { get; set; }
        //public DbSet<Department> Departments { get; set; }
        public DbSet<PatientDetails> PatientDetails { get; set; }

        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<AppointmentPayment> AppointmentPayment { get; set; }
        public DbSet<Prescription> Prescription { get; set; }
        public DbSet<PrescriptionMedicine> PrescriptionMedicine { get; set; }
        public DbSet<LabTest> LabTests { get; set; }
        public DbSet<LabTestItem> labTestItems { get; set; }
        public DbSet<LabPayment> LabPayments { get; set; }
        public DbSet<Labtechnicians> Labtechnicians { get; set; }
        public DbSet<ReceptionistDetails> receptionistDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Appointments <-> Doctor
            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Doctor)
                .WithMany(u => u.DoctorAppointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointments <-> Patient
            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Patient)
                .WithMany(u => u.PatientAppointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // existing LabTest relationships
            modelBuilder.Entity<LabTest>()
                .HasOne(l => l.Doctor)
                .WithMany()
                .HasForeignKey(l => l.DoctorUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabTest>()
                .HasOne(l => l.Patient)
                .WithMany()
                .HasForeignKey(l => l.PatientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabTest>()
                .HasOne(l => l.Appointment)
                .WithMany()
                .HasForeignKey(l => l.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Receptionist)
                .WithMany(r => r.ReceptionistAppointments)
                .HasForeignKey(a => a.ReceptionistId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointments>()
               .HasOne(a => a.Labtechnician)
               .WithMany(r => r.LabTechnicianAppointments)
               .HasForeignKey(a => a.LabTechnicianId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LabPayment>()
                .HasOne(p => p.Appointment)
                .WithMany()
                .HasForeignKey(p => p.AppointmentId);


        }
    }
}

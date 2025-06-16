using AutoMapper;
using Hospital.Db.Models;
using Hospital.Db.Models.Appointment;
using Hospital.Db.Models.Doctor;
using Hospital.Db.Models.Nurse;
using Hospital.Db.Models.Patients;
using Hospital.Db.Models.Receptionist;
using Hospital.Dto.Application;
using Hospital.Dto.Application.Doctor;
using Hospital.Dto.Application.Nurse;
using Hospital.Dto.Application.Patient;
using Hospital.Dto.Application.Receptionist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.BL.MappingProfile
{
    public class AutomapperProfle : Profile
    {
        public AutomapperProfle()
        {

            CreateMap<DoctorDetails, DoctorDetailsDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.firstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.lastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                .ForMember(dest => dest.AdharNo, opt => opt.MapFrom(src => src.AppUser.AdharNo))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                .ForMember(dest => dest.LicenseNumber, opt => opt.MapFrom(src => src.LicenseNumber))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.AppUser.DateOfBirth))
                .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.Qualification))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ReverseMap();

            CreateMap<PatientDetails, PatientsDetailsDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.patientId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.firstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.lastName))
                .ForMember(dest => dest.AdharNo, opt => opt.MapFrom(src => src.AppUser.AdharNo))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.AppUser.DateOfBirth))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ReverseMap();


            CreateMap<NurseDetails, NurseDetailsDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.nurseId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.firstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.lastName))
                .ForMember(dest => dest.AdharNo, opt => opt.MapFrom(src => src.AppUser.AdharNo))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.Email))
                .ReverseMap();
            CreateMap<NurseDetails, AddNurseDetailsDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.Qualification))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.AppUser.DateOfBirth))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.firstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.lastName))
                .ForMember(dest => dest.AdharNo, opt => opt.MapFrom(src => src.AppUser.AdharNo))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                .ForMember(dest => dest.JoiningDate, opt => opt.MapFrom(src => src.JoiningDate))
                .ForMember(dest => dest.Shift, opt => opt.MapFrom(src => src.Shift));

            CreateMap<AddNurseDetailsDto, NurseDetails>()
                .ForMember(dest => dest.AppUser, opt => opt.Ignore());

            CreateMap<DoctorDetails, AddDoctorDetailsDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.firstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.lastName))
                .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.Qualification))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.AppUser.DateOfBirth))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                .ForMember(dest => dest.AdharNO, opt => opt.MapFrom(src => src.AppUser.AdharNo))
                .ForMember(dest => dest.LicenseNumber, opt => opt.MapFrom(src => src.LicenseNumber))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender));

            CreateMap<AddDoctorDetailsDto, DoctorDetails>()
                .ForMember(dest => dest.AppUser, opt => opt.Ignore());


            CreateMap<PatientDetails, AddPatientDetailsDto>()
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.firstName))
                 .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.lastName))
                 .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.AppUser.DateOfBirth))
                 .ForMember(dest => dest.AdharNo, opt => opt.MapFrom(src => src.AppUser.AdharNo))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                 .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                 .ForMember(dest => dest.KnownAllergies, opt => opt.MapFrom(src => src.KnownAllergies))
                 .ForMember(dest => dest.BloodGroup, opt => opt.MapFrom(src => src.BloodGroup))
                 .ForMember(dest => dest.EmergencyContactNumber, opt => opt.MapFrom(src => src.EmergencyContactNumber))
                 .ForMember(dest => dest.MedicalHistory, opt => opt.MapFrom(src => src.MedicalHistory));

            CreateMap<AddPatientDetailsDto, PatientDetails>()
                .ForMember(dest => dest.AppUser, opt => opt.Ignore());

            CreateMap<ReceptionistDetails, AddReceptionistDetailsDto>()
                 .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.AppUser.firstName))
                 .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.AppUser.lastName))
                 .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.AppUser.DateOfBirth))
                 .ForMember(dest => dest.AdharNo, opt => opt.MapFrom(src => src.AppUser.AdharNo))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AppUser.Address))
                 .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                 .ForMember(dest => dest.ExperienceYear, opt => opt.MapFrom(src => src.ExperienceYear))
                 .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.Qualification));

            CreateMap<AddReceptionistDetailsDto, ReceptionistDetails>()
                .ForMember(dest => dest.AppUser, opt => opt.Ignore());


            CreateMap<Appointments, BookAppointmentDto>()
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.DoctorId, opt => opt.MapFrom(src => src.DoctorId))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason))
                .ReverseMap();

            CreateMap<Appointments, GetAppointmentDetailsDto>().ReverseMap();   

            CreateMap<Appointments, BookAppoinmentUpdateDto>()
                .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ReverseMap();


            CreateMap<DoctorDetails, SearchDoctorByPatientDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.AppUser.firstName+" "+ src.AppUser.lastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
                .ReverseMap();

            CreateMap<Prescription, WritePrescriptionDto>().ReverseMap()
            .ForMember(dest => dest.Advice, opt => opt.MapFrom(src => src.Advice))
            .ForMember(dest => dest.FollowUpDate, opt => opt.MapFrom(src => src.FollowUpDate))
            .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis))
            .ForMember(dest => dest.Symptoms, opt => opt.MapFrom(src => src.Symptoms)).ReverseMap();

            CreateMap<PrescriptionMedicine, MedicineDto>().ReverseMap();


            CreateMap<Prescription, GetPrescriptionDto>()
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.Advice, opt => opt.MapFrom(src => src.Advice))
                .ForMember(dest => dest.FollowUpDate, opt => opt.MapFrom(src => src.FollowUpDate))
                .ForMember(dest => dest.Diagnosis, opt => opt.MapFrom(src => src.Diagnosis))
                .ForMember(dest => dest.Symptoms, opt => opt.MapFrom(src => src.Symptoms))
                .ForMember(dest => dest.Medicines, opt => opt.MapFrom(src => src.Medicines))
                .ReverseMap();

            CreateMap<AppointmentPayment, PaymentDto>()
                .ForMember(dest => dest.appointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.paymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ReverseMap();
        }
    }
}

using Hospital.App.TokenExtension;
using Hospital.BL.Interface.Application;
using Hospital.BL.Interface.Application.Admin;
using Hospital.BL.Interface.Application.Doctor;
using Hospital.BL.Interface.Application.Email;
using Hospital.BL.Interface.Application.LabTechnician;
using Hospital.BL.Interface.Application.Nurse;
using Hospital.BL.Interface.Application.Patient;
using Hospital.BL.Interface.Application.Receptionist;
using Hospital.BL.Service.Application;
using Hospital.BL.Service.Application.Admin;
using Hospital.BL.Service.Application.Doctor;
using Hospital.BL.Service.Application.Email;
using Hospital.BL.Service.Application.LabTechnician;
using Hospital.BL.Service.Application.Nurse;
using Hospital.BL.Service.Application.Patient;
using Hospital.BL.Service.Application.Receptionist;
using Hospital.Db.AppLicationDbContext;
using Hospital.Db.Models;
using Hospital.Dto.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("Hospital.App"));
});

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<INurseService, NurseService>();
builder.Services.AddScoped<IReceptionistService, ReceptionistService>();
builder.Services.AddScoped<ILabTechnicianService, LabTechnicianService>();

builder.Services.AddScoped<IEmailService, EmailService>();
//builder.Services.AddAuthentication();

builder.Services.AddIdentity<AppUsers, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.AddTokenConfiguration();
builder.Services.AddAuthentication();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Doctor", policy => policy.RequireRole("Doctor"));
//    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("Patient", policy => policy.RequireRole("Patient"));
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

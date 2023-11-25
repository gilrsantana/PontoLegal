using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Repository;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service;
using PontoLegal.Service.Interfaces;
using PontoLegal.Service.Models;

var builder = WebApplication.CreateBuilder(args);

var connection = builder
 .Configuration
 .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PontoLegalContext>(options =>
options.UseSqlite(connection));

builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddScoped<IJobPositionService, JobPositionService>();
builder.Services.AddScoped<IJobPositionRepository, JobPositionRepository>();

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<IWorkingDayService, WorkingDayService>();
builder.Services.AddScoped<IWorkingDayRepository, WorkingDayRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<ITimeClockService, TimeClockService>();
builder.Services.AddScoped<ITimeClockRepository, TimeClockRepository>();

builder.Services.AddScoped<ITimeClockNotificationService, TimeClockNotificationService>();
builder.Services.AddScoped<ITimeClockNotificationRepository, TimeClockNotificationRepository>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Repository;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service;
using PontoLegal.Service.Interfaces;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

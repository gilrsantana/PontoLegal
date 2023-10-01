using Microsoft.EntityFrameworkCore;
using PontoLegal.Data.Mapping;
using PontoLegal.Domain.Entities;

namespace PontoLegal.Data;

public class PontoLegalContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<JobPosition> JobPositions { get; set; }
    public DbSet<TimeClock> TimeClocks { get; set; }
    public DbSet<WorkingDay> WorkingDays { get; set; }

    public PontoLegalContext(DbContextOptions<PontoLegalContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyMap());
        modelBuilder.ApplyConfiguration(new DepartmentMap());
        modelBuilder.ApplyConfiguration(new EmployeeMap());
        modelBuilder.ApplyConfiguration(new JobPositionMap());
        modelBuilder.ApplyConfiguration(new TimeClockMap());
        modelBuilder.ApplyConfiguration(new WorkingDayMap());
    }
}
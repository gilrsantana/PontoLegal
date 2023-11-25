using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PontoLegal.Domain.Entities;

namespace PontoLegal.Data.Mapping;

public class EmployeeMap : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("TEXT")
            .HasMaxLength(36)
            .HasConversion<string>();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType("TEXT")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.HireDate)
            .HasColumnName("HireDate")
            .HasColumnType("TEXT");

        builder.Property(x => x.RegistrationNumber)
            .HasColumnName("RegistrationNumber")
            .HasColumnType("TEXT")
            .HasMaxLength(20);

        builder.HasOne(x => x.JobPosition)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.JobPositionId)
            .IsRequired();

        builder.OwnsOne(x => x.Pis)
            .Property(x => x.Number)
            .HasColumnName("Pis")
            .HasColumnType("TEXT")
            .HasMaxLength(11)
            .IsRequired();

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.CompanyId)
            .IsRequired();

        builder.HasOne(x => x.WorkingDay)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.WorkingDayId)
            .IsRequired();

        builder.Property(x => x.ManagerId)
            .IsRequired(false)
            .HasColumnName("ManagerId")
            .HasColumnType("TEXT")
            .HasMaxLength(36)
            .HasConversion<string>();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME")
            .IsRequired();
    }
}
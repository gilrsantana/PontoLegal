using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PontoLegal.Domain.Entities;

namespace PontoLegal.Data.Mapping;

public class TimeClockMap : IEntityTypeConfiguration<TimeClock>
{
    public void Configure(EntityTypeBuilder<TimeClock> builder)
    {
        builder.ToTable("TimeClock");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("TEXT")
            .HasMaxLength(36)
            .HasConversion<string>();

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.TimeClocks)
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.RegisterType)
            .HasColumnName("RegisterType")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(x => x.ClockTimeStatus)
            .HasColumnName("ClockTimeStatus")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME")
            .IsRequired();
    }
}
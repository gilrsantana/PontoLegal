using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PontoLegal.Domain.Entities;

namespace PontoLegal.Data.Mapping;

public class WorkingDayMap : IEntityTypeConfiguration<WorkingDay>
{
    public void Configure(EntityTypeBuilder<WorkingDay> builder)
    {
        builder.ToTable("WorkingDay");

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("TEXT")
            .HasMaxLength(36)
            .HasConversion<string>();

        builder.Property(x => x.Name)
            .HasColumnName("Name")
            .HasColumnType("TEXT")
            .HasMaxLength(100);


        builder.Property(x => x.Type)
            .HasColumnName("Type")
            .HasColumnType("INTEGER")
            .HasConversion<int>();

        builder.Property(x => x.StartWork)
            .HasColumnName("StartWork")
            .HasColumnType("TEXT")
            .HasMaxLength(5)
            .HasConversion<string>();

        builder.Property(x => x.StartBreak)
            .HasColumnName("StartBreak")
            .HasColumnType("TEXT")
            .HasMaxLength(5)
            .HasConversion<string>();

        builder.Property(x => x.EndBreak)
            .HasColumnName("EndBreak")
            .HasColumnType("TEXT")
            .HasMaxLength(5)
            .HasConversion<string>();

        builder.Property(x => x.EndWork)
            .HasColumnName("EndWork")
            .HasColumnType("TEXT")
            .HasMaxLength(5)
            .HasConversion<string>();

        builder.Property(x => x.MinutesTolerance)
            .HasColumnName("MinutesTolerance")
            .HasColumnType("INTEGER");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME")
            .IsRequired();
    }
}

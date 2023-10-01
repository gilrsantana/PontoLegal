using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PontoLegal.Domain.Entities;

namespace PontoLegal.Data.Mapping;

public class DepartmentMap : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Department");

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

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME")
            .IsRequired();
    }
}
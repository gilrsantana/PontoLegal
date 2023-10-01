using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PontoLegal.Domain.Entities;

namespace PontoLegal.Data.Mapping;

public class CompanyMap : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Company");

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

        builder.OwnsOne(x => x.Cnpj, cnpj =>
        {
            cnpj.Property(x => x.Number)
                .HasColumnName("Cnpj")
                .HasColumnType("TEXT")
                .HasMaxLength(14)
                .IsRequired();
        });

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("DATETIME")
            .IsRequired();
    }
}
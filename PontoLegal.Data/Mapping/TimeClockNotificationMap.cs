using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PontoLegal.Domain.Entities;

namespace PontoLegal.Data.Mapping;

public class TimeClockNotificationMap : IEntityTypeConfiguration<TimeClockNotification>
{
    public void Configure(EntityTypeBuilder<TimeClockNotification> builder)
    {
        builder.ToTable("TimeClockNotification");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("TEXT")
            .HasMaxLength(36)
            .HasConversion<string>();

        builder.Property(x => x.NotificationStatus)
            .HasColumnName("NotificationStatus")
            .HasColumnType("integer")
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(x => x.TimeClock)
            .WithMany(x => x.TimeClockNotifications)
            .HasForeignKey(x => x.TimeClockId)
            .HasConstraintName("FK_TimeClockNotification_TimeClock")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("datetime")
            .IsRequired();
    }
}
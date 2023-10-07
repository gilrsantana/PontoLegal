using PontoLegal.Domain.Enums;

namespace PontoLegal.Domain.Entities;

public class TimeClockNotification : BaseEntity
{
    public Guid TimeClockId { get; private set; }
    public TimeClock? TimeClock { get; private set; }
    public NotificationStatus NotificationStatus { get; private set; }

    public TimeClockNotification(Guid timeClockId, NotificationStatus notificationStatus)
    {
        TimeClockId = timeClockId;
        NotificationStatus = notificationStatus;
    }
}
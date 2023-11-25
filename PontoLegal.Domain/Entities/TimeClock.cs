using PontoLegal.Domain.Enums;

namespace PontoLegal.Domain.Entities;

public delegate Task TimeClockAddedEventHandler(object sender, EventArgs e);

public class TimeClock : BaseEntity
{
    public DateTime RegisterTime { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }
    public RegisterType RegisterType { get; private set; }
    public ClockTimeStatus ClockTimeStatus { get; private set; }
    public ICollection<TimeClockNotification> TimeClockNotifications { get; private set; }

    public event TimeClockAddedEventHandler? TimeClockAddedEventHandler;

    public TimeClock(Guid employeeId, RegisterType registerType)
    {
        RegisterTime = DateTime.Now;
        EmployeeId = employeeId;
        RegisterType = registerType;
        ClockTimeStatus = ClockTimeStatus.APPROVED;
        TimeClockAddedEventHandler?.Invoke(this, EventArgs.Empty);
    }

    public void SetClockTimeStatus(ClockTimeStatus clockTimeStatus)
    {
        ClockTimeStatus = clockTimeStatus;
    }

}

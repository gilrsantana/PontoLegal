using PontoLegal.Domain.Enums;

namespace PontoLegal.Domain.Entities;

public class TimeClock : BaseEntity
{
    public DateTime RegisterTime { get; private set; }
    public Guid EmployeeId { get; private set; }
    public RegisterType RegisterType { get; private set; }
    public ClockTimeStatus ClockTimeStatus { get; private set; }

    public TimeClock(DateTime registerTime, Guid employeeId, RegisterType registerType, ClockTimeStatus clockTimeStatus)
    {
        RegisterTime = registerTime;
        EmployeeId = employeeId;
        RegisterType = registerType;
        ClockTimeStatus = clockTimeStatus;
    }
}
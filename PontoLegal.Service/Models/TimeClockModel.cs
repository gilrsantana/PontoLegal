using Flunt.Validations;
using PontoLegal.Domain.Enums;

namespace PontoLegal.Service.Models;

public class TimeClockModel : BaseModel
{
    public DateTime RegisterTime { get; private set; }
    public Guid EmployeeId { get; private set; }
    public RegisterType RegisterType { get; private set; }

    public TimeClockModel( Guid employeeId, RegisterType registerType)
    {
        RegisterTime = DateTime.Now;
        EmployeeId = employeeId;
        RegisterType = registerType;
        AddNotifications(new Contract<TimeClockModel>()
            .Requires()
            .IsTrue(Guid.Empty != EmployeeId, "TimeClock.EmployeeId", "EmployeeId is required"));
    }
}
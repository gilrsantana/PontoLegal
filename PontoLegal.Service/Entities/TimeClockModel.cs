using Flunt.Validations;
using PontoLegal.Domain.Enums;

namespace PontoLegal.Service.Entities;

public class TimeClockModel : BaseModel
{
    public DateTime RegisterTime { get; private set; }
    public Guid EmployeeId { get; private set; }
    public RegisterType RegisterType { get; private set; }

    public TimeClockModel(DateTime registerTime, Guid employeeId, RegisterType registerType)
    {
        RegisterTime = registerTime;
        EmployeeId = employeeId;
        RegisterType = registerType;
        AddNotifications(new Contract<TimeClockModel>()
            .Requires()
            .IsLowerOrEqualsThan(RegisterTime, DateTime.Now, "TimeClockModel.RegisterTime",
                "RegisterTime must be lower or equals than now")
            .IsTrue(Guid.Empty != EmployeeId, "TimeClock.EmployeeId", "EmployeeId is required"));
    }
}
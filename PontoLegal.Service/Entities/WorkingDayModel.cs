using Flunt.Validations;
using PontoLegal.Domain.Enums;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service.Entities;

public class WorkingDayModel : BaseModel
{
    public string Name { get; }
    public WorkingDayType Type { get; }
    public TimeOnly StartWork { get; }
    public TimeOnly StartBreak { get; }
    public TimeOnly EndBreak { get; }
    public TimeOnly EndWork { get; }
    public WorkingDayModel(
        string name, 
        WorkingDayType type, 
        TimeOnly startWork, 
        TimeOnly startBreak, 
        TimeOnly endBreak, 
        TimeOnly endWork)
    {
        Name = name;
        Type = type;
        StartWork = startWork;
        StartBreak = startBreak;
        EndBreak = endBreak;
        EndWork = endWork;
        
        AddNotifications(new Contract<WorkingDayModel>()
            .Requires()
            .IsGreaterOrEqualsThan(Name, 3, "WorkingDay.Name", Error.WorkingDay.INVALID_NAME)
            .IsLowerOrEqualsThan(Name, 30, "WorkingDay.Name", Error.WorkingDay.INVALID_NAME)
            .IsTrue(StartWork < EndWork && StartWork < StartBreak, 
                "WorkingDay.StartWork", Error.WorkingDay.INVALID_START_WORK)
            .IsTrue(StartBreak < EndBreak && StartBreak < EndWork, 
                "WorkingDay.StartBreak", Error.WorkingDay.INVALID_START_BREAK)
            .IsTrue(EndBreak < EndWork, 
                "WorkingDay.EndBreak", Error.WorkingDay.INVALID_END_BREAK)
            .IsTrue((int)Type == GetDiffBetweenStartAndEndWork(), 
                "WorkingDay.Type", Error.WorkingDay.INVALID_TYPE)
        );
    }
    
    private int GetDiffBetweenStartAndEndWork()
    {
        var diff = EndWork - StartWork;
        return (int)diff.TotalMinutes / 60;
    }
}
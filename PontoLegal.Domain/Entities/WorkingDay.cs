using PontoLegal.Domain.Enums;

namespace PontoLegal.Domain.Entities;

public class WorkingDay : BaseEntity
{
    public string Name { get; private set; }
    public WorkingDayType Type { get; private set; }
    public TimeOnly StartWork { get; private set; }
    public TimeOnly StartBreak { get; private set; }
    public TimeOnly EndBreak { get; private set; }
    public TimeOnly EndWork { get; private set; }

    public WorkingDay(
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
    }
}
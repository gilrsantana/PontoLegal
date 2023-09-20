using PontoLegal.Domain.Enums;

namespace PontoLegal.Domain.Entities;

public class WorkingDay : BaseEntity
{
    public string Name { get; set; }
    public WorkingDayType Type { get; set; }
    public TimeOnly StartWork { get; set; }
    public TimeOnly StartBreak { get; set; }
    public TimeOnly EndBreak { get; set; }
    public TimeOnly EndWork { get; set; }

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
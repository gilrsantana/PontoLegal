using PontoLegal.Domain.Enums;

namespace PontoLegal.Service.DTOs;

public class WorkingDayDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public WorkingDayType Type { get; set; }
    public TimeOnly StartWork { get; set; }
    public TimeOnly StartBreak { get; set; }
    public TimeOnly EndBreak { get; set; }
    public TimeOnly EndWork { get; set; }
}
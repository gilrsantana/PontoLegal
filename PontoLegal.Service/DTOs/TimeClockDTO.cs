using PontoLegal.Domain.Enums;

namespace PontoLegal.Service.DTOs;

public class TimeClockDTO
{
    public Guid Id { get; set; }
    public DateTime RegisterTime { get; set; }
    public Guid EmployeeId { get; set; }
    public RegisterType RegisterType { get; set; }
    public ClockTimeStatus ClockTimeStatus { get; set; }
}
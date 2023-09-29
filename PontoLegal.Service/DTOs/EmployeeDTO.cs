namespace PontoLegal.Service.DTOs;

public class EmployeeDTO
{
    public Guid EmployeeId { get; set; }
    public string Name { get; set; } = "";
    public DateOnly HireDate { get; set; }
    public string RegistrationNumber { get; set; } = "";
    public Guid JobPositionId { get; set; }
    public string PisNumber { get; set; } = "";
    public Guid CompanyId { get; set; }
    public Guid ManagerId { get; set; }
    public Guid WorkingDayId { get; set; }
}
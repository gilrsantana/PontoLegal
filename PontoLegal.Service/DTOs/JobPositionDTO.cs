namespace PontoLegal.Service.DTOs;

public class JobPositionDTO
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; } = new();
    public string Name { get; set; } = string.Empty;
}
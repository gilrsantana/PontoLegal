using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Domain.Entities;

public class Employee : BaseEntity
{
    public string  Name { get; set; }
    public DateOnly HireDate { get; set; }
    public string RegistrationNumber { get; set; }
    public Guid JobPositionId { get; set; }
    public Pis Pis { get; set; }
    public Guid CompanyId { get; set; }
    public Guid ManagerId { get; set; }
    public Guid WorkingDayId { get; set; }

    public Employee(
        string name, 
        DateOnly hireDate, 
        string registrationNumber, 
        Guid jobPositionId, 
        Pis pis, 
        Guid companyId, 
        Guid managerId, 
        Guid workingDayId)
    {
        Name = name;
        HireDate = hireDate;
        RegistrationNumber = registrationNumber;
        JobPositionId = jobPositionId;
        Pis = pis;
        CompanyId = companyId;
        ManagerId = managerId;
        WorkingDayId = workingDayId;
    }
}
using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Domain.Entities;

public class Employee : BaseEntity
{
    public string  Name { get; private set; }
    public DateOnly HireDate { get; private set; }
    public string RegistrationNumber { get; private set; }
    public Guid JobPositionId { get; private set; }
    public JobPosition JobPosition { get; private set; }
    public Pis Pis { get; private set; }
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; }
    public Guid? ManagerId { get; private set; }
    public Guid WorkingDayId { get; private set; }
    public WorkingDay WorkingDay { get; private set; }
    public ICollection<TimeClock> TimeClocks { get; private set; }

    public Employee()
    {
        
    }
    public Employee(
        string name, 
        DateOnly hireDate, 
        string registrationNumber, 
        Guid jobPositionId, 
        string pis, 
        Guid companyId, 
        Guid? managerId, 
        Guid workingDayId)
    {
        Name = name;
        HireDate = hireDate;
        RegistrationNumber = registrationNumber;
        JobPositionId = jobPositionId;
        Pis = new Pis(pis);
        CompanyId = companyId;
        ManagerId = managerId;
        WorkingDayId = workingDayId;
    }

    public void Update(
        string name,
        DateOnly hireDate,
        string registrationNumber,
        Guid jobPositionId,
        string pis,
        Guid companyId,
        Guid? managerId,
        Guid workingDayId)
    {
        Name = name;
        HireDate = hireDate;
        RegistrationNumber = registrationNumber;
        JobPositionId = jobPositionId;
        Pis = new Pis(pis);
        CompanyId = companyId;
        ManagerId = managerId;
        WorkingDayId = workingDayId;
    }
}
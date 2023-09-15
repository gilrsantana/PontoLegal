namespace PontoLegal.Domain.Entities;

public class JobPosition : BaseEntity
{
    public string Name { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; }

    public JobPosition(string name, Guid departmentId)
    {
        Name = name;
        DepartmentId = departmentId;
    }

    public JobPosition(string name, Guid departmentId, Department department)
    {
        Name = name;
        DepartmentId = departmentId;
        Department = department;
    }
}
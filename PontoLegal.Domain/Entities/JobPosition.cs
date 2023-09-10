namespace PontoLegal.Domain.Entities;

public class JobPosition : BaseEntity
{
    public string Name { get; private set; }
    public Department Department { get; private set; }

    public JobPosition(string name, Department department)
    {
        Name = name;
        Department = department;
    }
}
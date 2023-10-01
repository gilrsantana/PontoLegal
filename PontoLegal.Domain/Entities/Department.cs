namespace PontoLegal.Domain.Entities;
public class Department : BaseEntity
{
    public string Name { get; private set; }
    public ICollection<JobPosition> JobPositions { get; private set; }

    public Department(string name)
    {
        Name = name;
    }
    public Department(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}

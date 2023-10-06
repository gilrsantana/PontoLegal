using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public ICollection<Employee>? Employees { get; private set; }

    public Company() { }

    public Company(string name, string cnpj)
    {
        Name = name;
        Cnpj = new Cnpj(cnpj);
    }
    public void Update(string name, Cnpj cnpj)
    {
        Name = name;
        Cnpj = cnpj;
    }
}
using PontoLegal.Domain.ValueObjects;

namespace PontoLegal.Domain.Entities;

public class Company : BaseEntity
{
    public string Name { get; private set; }
    public Cnpj Cnpj { get; private set; }

    public Company(string name, Cnpj cnpj)
    {
        Name = name;
        Cnpj = cnpj;
    }
}
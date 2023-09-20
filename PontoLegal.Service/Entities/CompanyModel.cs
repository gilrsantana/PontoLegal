using Flunt.Validations;
using PontoLegal.Domain.ValueObjects;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service.Entities;

public class CompanyModel : BaseModel
{
    public string Name { get; }
    public Cnpj Cnpj { get; }

    public CompanyModel(string name, string cnpj)
    {
        Name = name;
        Cnpj = new Cnpj(cnpj);
        AddNotifications(new Contract<CompanyModel>()
            .Requires()
            .IsGreaterOrEqualsThan(Name, 3, "CompanyModel.Name", Error.Company.INVALID_NAME)
            .IsLowerOrEqualsThan(Name, 30, "CompanyModel.Name", Error.Company.INVALID_NAME));
        ValidateCnpj(cnpj);
    }

    private void ValidateCnpj(string cnpj)
    {
        if (Cnpj.IsValid) return;
        foreach (var error in Cnpj.GetErrors())
        {
            AddNotification("CompanyModel.Cnpj", error);
        }
    }

}
using Flunt.Validations;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service.Models;

public class CompanyModel : BaseModel
{
    public string Name { get; }
    public string Cnpj { get; }

    public CompanyModel(string name, string cnpj)
    {
        Name = name;
        Cnpj = cnpj;
        AddNotifications(new Contract<CompanyModel>()
            .Requires()
            .IsGreaterOrEqualsThan(Name, 3, "CompanyModel.Name", Error.Company.INVALID_NAME)
            .IsLowerOrEqualsThan(Name, 30, "CompanyModel.Name", Error.Company.INVALID_NAME)
            .IsNotNullOrEmpty(Cnpj, "CompanyModel.Cnpj", Error.Company.INVALID_CNPJ));
    }
}
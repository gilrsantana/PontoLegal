using Flunt.Validations;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service.Models;

public class DepartmentModel : BaseModel
{
    public string Name { get; set; }

    public DepartmentModel(string name)
    {
        Name = name;
        AddNotifications(new Contract<DepartmentModel>()
            .Requires()
            .IsGreaterOrEqualsThan(Name, 3, "Department.Name", Error.Department.INVALID_NAME)
            .IsLowerOrEqualsThan(Name, 30, "Department.Name", Error.Department.INVALID_NAME)
        );
    }
}
using Flunt.Validations;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service.Entities;

public class JobPositionModel : BaseModel
{
    public string Name { get; set; }
    public DepartmentModel Department { get; set; }

    public JobPositionModel(string name, DepartmentModel department)
    {
        Name = name;
        Department = department;
        AddNotifications(new Contract<JobPositionModel>()
            .Requires()
            .IsGreaterOrEqualsThan(Name, 3, "JobPosition.Name", Error.JobPosition.INVALID_NAME)
            .IsLowerOrEqualsThan(Name, 30, "JobPosition.Name", Error.JobPosition.INVALID_NAME)
        );
        AddNotifications(department);
    }
}
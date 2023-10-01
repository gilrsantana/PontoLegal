using Flunt.Validations;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service.Models;

public class JobPositionModel : BaseModel
{
    public string Name { get; set; }
    public Guid DepartmentId { get; set; }

    public JobPositionModel(string name, Guid departmentId)
    {
        Name = name;
        DepartmentId = departmentId;
        AddNotifications(new Contract<JobPositionModel>()
            .Requires()
            .IsGreaterOrEqualsThan(Name, 3, "JobPosition.Name", Error.JobPosition.INVALID_NAME)
            .IsLowerOrEqualsThan(Name, 30, "JobPosition.Name", Error.JobPosition.INVALID_NAME)
        );
    }
}
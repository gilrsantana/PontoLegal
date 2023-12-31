using Flunt.Validations;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service.Models;

public class EmployeeModel : BaseModel
{
    public string  Name { get; set; }
    public DateOnly HireDate { get; set; }
    public string RegistrationNumber { get; set; }
    public Guid JobPositionId { get; set; }
    public string Pis { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid WorkingDayId { get; set; }

    public EmployeeModel(
        string name,
        DateOnly hireDate,
        string registrationNumber,
        Guid jobPositionId,
        string pis,
        Guid companyId,
        Guid? managerId,
        Guid workingDayId)
    {
        Name = name;
        HireDate = hireDate;
        RegistrationNumber = registrationNumber;
        JobPositionId = jobPositionId;
        Pis = pis;
        CompanyId = companyId;
        ManagerId = managerId;
        WorkingDayId = workingDayId;
        AddNotifications(new Contract<EmployeeModel>()
            .Requires()
            .IsGreaterOrEqualsThan(Name, 3, "EmployeeModel.Name", Error.Employee.INVALID_NAME)
            .IsLowerOrEqualsThan(Name, 80, "EmployeeModel.Name", Error.Employee.INVALID_NAME)
            .IsGreaterOrEqualsThan(RegistrationNumber, 1, "EmployeeModel.RegistrationNumber", Error.Employee.INVALID_REGISTRATION_NUMBER)
            .IsLowerOrEqualsThan(RegistrationNumber, 20, "EmployeeModel.RegistrationNumber", Error.Employee.INVALID_REGISTRATION_NUMBER)
            .IsTrue(HireDate <= DateOnly.FromDateTime(DateTime.Now), "EmployeeModel.HireDate", Error.Employee.INVALID_HIRE_DATE)
            .IsTrue(JobPositionId != Guid.Empty, "EmployeeModel.JobPositionId", Error.Employee.INVALID_JOB_POSITION_ID)
            .IsNotNullOrEmpty(Pis, "EmployeeModel.Pis", Error.Employee.INVALID_PIS)
            .IsTrue(CompanyId != Guid.Empty, "EmployeeModel.CompanyId", Error.Employee.INVALID_COMPANY_ID)
            .IsTrue(WorkingDayId != Guid.Empty, "EmployeeModel.WorkingDayId", Error.Employee.INVALID_WORKING_DAY_ID)
        );
    }
    

}
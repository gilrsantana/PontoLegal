using PontoLegal.Domain.Entities;
using PontoLegal.Domain.ValueObjects;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;
using PontoLegal.Service.Interfaces;
using PontoLegal.Shared.Messages;

namespace PontoLegal.Service;

public class EmployeeService : BaseService, IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IJobPositionService _jobPositionService;
    private readonly ICompanyService _companyService;
    private readonly IWorkingDayService _workingDayService;
    public EmployeeService(IEmployeeRepository employeeRepository, 
        IJobPositionService jobPositionService, 
        ICompanyService companyService, IWorkingDayService workingDayService)
    {
        _employeeRepository = employeeRepository;
        _jobPositionService = jobPositionService;
        _companyService = companyService;
        _workingDayService = workingDayService;
    }
    
    public async Task<bool> AddEmployeeAsync(EmployeeModel model)
    {
        if (!model.IsValid)
        {
            AddNotifications(model.Notifications);
            return false;
        }
        
        var jobPosition = await _jobPositionService.GetJobPositionByIdAsync(model.JobPositionId);
        if (jobPosition == null)
        {
            AddNotification("EmployeeService.JobPositionId", Error.Employee.JOB_POSITION_NOT_FOUNDED);
            return false;
        }
        
        var company = await _companyService.GetCompanyByIdAsync(model.CompanyId);
        if (company == null)
        {
            AddNotification("EmployeeService.CompanyId", Error.Employee.COMPANY_NOT_FOUNDED);
            return false;
        }
        
        var workingDay = await _workingDayService.GetWorkingDayByIdAsync(model.WorkingDayId);
        if (workingDay == null)
        {
            AddNotification("EmployeeService.WorkingDayId", Error.Employee.WORKING_DAY_NOT_FOUNDED);
            return false;
        }
        
        var existingEmployee = await _employeeRepository.GetEmployeeByPisAsync(model.Pis.Number);
        if (existingEmployee != null)
        {
            AddNotification("EmployeeService.Pis", Error.Employee.PIS_ALREADY_EXISTS);
            return false;
        }
        
        var employee = new Employee(
            model.Name, 
            model.HireDate, 
            model.RegistrationNumber, 
            model.JobPositionId, 
            model.Pis, 
            model.CompanyId, 
            model.ManagerId, 
            model.WorkingDayId);
        
        var result = await _employeeRepository.AddEmployeeAsync(employee);
        if(result) return true;
        
        AddNotification("EmployeeService", Error.Employee.ERROR_ADDING);
        return false;
    }
    
    public async Task<EmployeeDTO?> GetEmployeeByPisAsync(string pisNumber)
    {
        var pis = new Pis(pisNumber);
        if (!pis.IsValid)
        {
            foreach (var error in pis.GetErrors())
            {
                AddNotification("EmployeeService.Pis",error);
            }
            return null;
        }
        
        var employee = await _employeeRepository.GetEmployeeByPisAsync(pisNumber);
        if (employee == null)
        {
            AddNotification("EmployeeService.Pis", Error.Employee.PIS_NOT_FOUNDED);
            return null;
        }
        
        return new EmployeeDTO
        {
            EmployeeId = employee.Id,
            Name = employee.Name,
            HireDate = employee.HireDate,
            RegistrationNumber = employee.RegistrationNumber,
            JobPositionId = employee.JobPositionId,
            PisNumber = employee.Pis.Number,
            CompanyId = employee.CompanyId,
            WorkingDayId = employee.WorkingDayId,
            ManagerId = employee.ManagerId
        };
    }

    public async Task<EmployeeDTO?> GetEmployeeByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            AddNotification("EmployeeService.Id", Error.Employee.INVALID_ID);
            return null;
        }
        
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            AddNotification("EmployeeService.Id", Error.Employee.EMPLOYEE_NOT_FOUNDED);
            return null;
        }
        
        return new EmployeeDTO
        {
            EmployeeId = employee.Id,
            Name = employee.Name,
            HireDate = employee.HireDate,
            RegistrationNumber = employee.RegistrationNumber,
            JobPositionId = employee.JobPositionId,
            PisNumber = employee.Pis.Number,
            CompanyId = employee.CompanyId,
            WorkingDayId = employee.WorkingDayId,
            ManagerId = employee.ManagerId
        };
    }

    public async Task<bool> RemoveEmployeeByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            AddNotification("EmployeeService.Id", Error.Employee.INVALID_ID);
            return false;
        }
        
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            AddNotification("EmployeeService.Id", Error.Employee.EMPLOYEE_NOT_FOUNDED);
            return false;
        }
        
        var result = await _employeeRepository.RemoveEmployeeByIdAsync(employee);
        if(result) return true;
        
        AddNotification("EmployeeService", Error.Employee.ERROR_REMOVING);
        return false;
    }
}



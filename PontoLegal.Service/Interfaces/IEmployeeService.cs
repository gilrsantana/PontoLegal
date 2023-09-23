using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface IEmployeeService
{
    Task<bool> AddEmployeeAsync(EmployeeModel model);
    
}
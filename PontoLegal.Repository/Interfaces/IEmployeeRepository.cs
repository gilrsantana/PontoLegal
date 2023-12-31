using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IEmployeeRepository
{ 
    Task<Employee?> GetEmployeeByIdAsync(Guid isAny);
    Task<Employee?> GetEmployeeByPisAsync(string pisNumber);
    Task<bool> AddEmployeeAsync(Employee employee);
    Task<bool> UpdateEmployeeAsync(Employee employee);
    Task<bool> RemoveEmployeeByIdAsync(Employee employee);
}
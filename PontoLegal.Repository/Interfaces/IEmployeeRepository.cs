using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IEmployeeRepository
{ 
    Task<Employee?> GetEmployeeByPisAsync(string pisNumber);
    Task<bool> AddEmployeeAsync(Employee employee);
}
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;

namespace PontoLegal.Service.Interfaces;

public interface IEmployeeService : IBaseService
{
    Task<EmployeeDTO?> GetEmployeeByIdAsync(Guid id);
    Task<EmployeeDTO?> GetEmployeeByPisAsync(string pisNumber);
    Task<bool> AddEmployeeAsync(EmployeeModel model);
    Task<bool> UpdateEmployeeAsync(Guid id, EmployeeModel model);
    Task<bool> RemoveEmployeeByIdAsync(Guid id);
}
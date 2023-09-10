using PontoLegal.Service.DTOs;
using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface IDepartmentService
{
    Task<bool> AddDepartmentAsync(DepartmentModel model); 
    Task<DepartmentDTO?> GetDepartmentByNameAsync(string departmentName);
    Task<DepartmentDTO?> GetDepartmentByIdAsync(Guid departmentId);
    Task<bool> UpdateDepartmentAsync(Guid id, DepartmentModel model);
    Task<bool> RemoveDepartmentByIdAsync(Guid id);
}
using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IDepartmentRepository
{
    Task<bool> AddDepartmentAsync(Department department);
    Task<bool> UpdateDepartmentAsync(Guid id, Department department);
    Task<Department?> GetDepartmentByNameAsync(string departmentName);
    Task<Department?> GetDepartmentByIdAsync(Guid departmentId);
    Task<bool> RemoveDepartmentByIdAsync(Guid id);
}

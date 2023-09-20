using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface IDepartmentRepository
{
    Task<Department?> GetDepartmentByIdAsync(Guid departmentId);
    Task<Department?> GetDepartmentByNameAsync(string departmentName);
    Task<ICollection<Department>?> GetAllDepartmentsAsync(int skip=0, int take=25);
    Task<bool> AddDepartmentAsync(Department department);
    Task<bool> UpdateDepartmentAsync(Guid id, Department department);
    Task<bool> RemoveDepartmentByIdAsync(Guid id);
}

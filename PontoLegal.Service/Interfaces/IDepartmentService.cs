using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;

namespace PontoLegal.Service.Interfaces;

public interface IDepartmentService : IBaseService
{
    Task<DepartmentDTO?> GetDepartmentByIdAsync(Guid departmentId);
    Task<DepartmentDTO?> GetDepartmentByNameAsync(string departmentName);
    Task<ICollection<DepartmentDTO>> GetAllDepartmentsAsync(int skip = 0, int take = 25);
    Task<bool> AddDepartmentAsync(DepartmentModel model); 
    Task<bool> UpdateDepartmentAsync(Guid id, DepartmentModel model);
    Task<bool> RemoveDepartmentByIdAsync(Guid id);
}
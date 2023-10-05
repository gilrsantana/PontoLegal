using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;

namespace PontoLegal.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly PontoLegalContext _context;

    public DepartmentRepository(PontoLegalContext context)
    {
        _context = context;
    }

    public async Task<Department?> GetDepartmentByIdAsync(Guid departmentId)
    {
        return await _context
            .Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == departmentId);
    }

    public async Task<Department?> GetDepartmentByNameAsync(string departmentName)
    {
        return await _context
            .Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Name == departmentName);
    }

    public async Task<ICollection<Department>?> GetAllDepartmentsAsync(int skip = 0, int take = 25)
    {
        return await _context
            .Departments
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<bool> AddDepartmentAsync(Department department)
    {
        _context.Departments.Add(department);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateDepartmentAsync(Department department)
    {
        _context.Departments.Update(department);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveDepartmentByIdAsync(Guid id)
    {
        var department = await GetDepartmentByIdAsync(id);
        if (department == null) return false;
            
        _context.Departments.Remove(department);
        return await _context.SaveChangesAsync() > 0;
    }
}
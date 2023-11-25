using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;

namespace PontoLegal.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly PontoLegalContext _context;

    public EmployeeRepository(PontoLegalContext context)
    {
        _context = context;
    }
    
    public async Task<Employee?> GetEmployeeByIdAsync(Guid id) =>
        await _context
            .Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Employee?> GetEmployeeByPisAsync(string pisNumber) =>
        await _context
            .Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Pis.Number == pisNumber);

    public async Task<bool> AddEmployeeAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveEmployeeByIdAsync(Employee employee)
    {
        _context.Employees.Remove(employee);
        return await _context.SaveChangesAsync() > 0;
    }
}
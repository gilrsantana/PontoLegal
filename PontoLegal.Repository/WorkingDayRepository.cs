using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;

namespace PontoLegal.Repository;

public class WorkingDayRepository : IWorkingDayRepository
{
    private readonly PontoLegalContext _context;

    public WorkingDayRepository(PontoLegalContext context)
    {
        _context = context;
    }

    public async Task<WorkingDay?> GetWorkingDayByIdAsync(Guid id)
    {
        return await _context
            .WorkingDays
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<WorkingDay?> GetWorkingDayByNameAsync(string name)
    {
        return await _context
            .WorkingDays
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<ICollection<WorkingDay>?> GetAllWorkingDaysAsync<TResult>(int skip = 0, int take = 25)
    {
        return await _context
            .WorkingDays
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<bool> AddWorkingDayAsync(WorkingDay workingDay)
    {
        _context.WorkingDays.Add(workingDay);                  
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateWorkingDayAsync( WorkingDay workingDay)
    {
        _context.WorkingDays.Update(workingDay);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveWorkingDayByIdAsync(Guid id)
    {
        var workingDay = await GetWorkingDayByIdAsync(id);

        if (workingDay == null)
            return false;

        _context.WorkingDays.Remove(workingDay);

        return await _context.SaveChangesAsync() > 0;
    }
}
using Microsoft.EntityFrameworkCore;
using PontoLegal.Data;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;

namespace PontoLegal.Repository;

public class TimeClockRepository : ITimeClockRepository
{
    private readonly PontoLegalContext _context;

    public TimeClockRepository(PontoLegalContext context)
    {
        _context = context;
    }

    public async Task<TimeClock?> GetTimeClockByIdAsync(Guid id) =>
        await _context
            .TimeClocks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ICollection<TimeClock>> GetTimeClocksByEmployeeIdAndDateAsync(Guid id, DateTime date) => 
        await _context
            .TimeClocks
            .AsNoTracking()
            .OrderBy(x => x.EmployeeId)
            .ThenBy(x => x.RegisterTime)
            .Where(x => x.EmployeeId == id && x.RegisterTime.Date == date.Date)
            .ToListAsync();
    
    public async Task<bool> AddTimeClockAsync(TimeClock timeClock)
    {
        _context.TimeClocks.Add(timeClock);
        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> UpdateTimeClockAsync(TimeClock timeClock)
    {
        _context.TimeClocks.Update(timeClock);
        return await _context.SaveChangesAsync() > 0;
    }
}
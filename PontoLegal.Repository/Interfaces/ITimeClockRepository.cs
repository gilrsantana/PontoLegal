using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface ITimeClockRepository
{
    Task<ICollection<TimeClock>> GetTimeClocksByEmployeeIdAndDateAsync(Guid id, DateTime date);
    Task<bool> AddTimeClockAsync(TimeClock timeClock);
    Task<TimeClock?> GetTimeClockByIdAsync(Guid id);
    Task<bool> UpdateTimeClockAsync(TimeClock timeClock);
}
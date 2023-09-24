using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface ITimeClockRepository
{
    Task<ICollection<TimeClock>> GetTimeClocksByEmployeeIdAndDateAsync(Guid id, DateTime date);
}
using PontoLegal.Domain.Enums;
using PontoLegal.Service.DTOs;
using PontoLegal.Service.Models;

namespace PontoLegal.Service.Interfaces;

public interface ITimeClockService : IBaseService
{
    Task<ICollection<TimeClockDTO>> GetTimeClocksByEmployeeIdAndDateAsync(Guid id, DateTime date);
    Task<bool> AddTimeClockAsync(TimeClockModel timeClock);
    Task<bool> UpdateTimeClockStatus(Guid timeClockId, ClockTimeStatus status);
}
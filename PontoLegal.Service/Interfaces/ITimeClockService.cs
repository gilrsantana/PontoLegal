using PontoLegal.Service.Entities;

namespace PontoLegal.Service.Interfaces;

public interface ITimeClockService
{
    Task<bool> AddTimeClockAsync(TimeClockModel timeClock);
}
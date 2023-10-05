using PontoLegal.Service.Models;

namespace PontoLegal.Service.Interfaces;

public interface ITimeClockService : IBaseService
{
    Task<bool> AddTimeClockAsync(TimeClockModel timeClock);
}
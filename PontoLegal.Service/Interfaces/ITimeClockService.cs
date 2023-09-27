using PontoLegal.Service.Models;

namespace PontoLegal.Service.Interfaces;

public interface ITimeClockService
{
    Task<bool> AddTimeClockAsync(TimeClockModel timeClock);
}
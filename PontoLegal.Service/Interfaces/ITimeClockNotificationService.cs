using PontoLegal.Domain.Entities;

namespace PontoLegal.Service.Interfaces;

public interface ITimeClockNotificationService
{
    Task<bool> AddNotificationAsync(TimeClockNotification notification);
}
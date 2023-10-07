using PontoLegal.Domain.Entities;

namespace PontoLegal.Repository.Interfaces;

public interface ITimeClockNotificationRepository
{ 
    Task<bool> AddNotificationAsync(TimeClockNotification notification);
} 
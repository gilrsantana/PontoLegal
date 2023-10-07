using Flunt.Notifications;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;
using PontoLegal.Service.Interfaces;

namespace PontoLegal.Service.Models;

public class TimeClockNotificationService : BaseService, ITimeClockNotificationService
{
    private readonly ITimeClockNotificationRepository _timeClockNotificationRepository;

    public TimeClockNotificationService(ITimeClockNotificationRepository timeClockNotificationRepository)
    {
        _timeClockNotificationRepository = timeClockNotificationRepository;
    }

    public async Task<bool> AddNotificationAsync(TimeClockNotification notification)
    {
        var result = await _timeClockNotificationRepository.AddNotificationAsync(notification);
        return result;
    }
}
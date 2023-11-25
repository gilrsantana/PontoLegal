using PontoLegal.Data;
using PontoLegal.Domain.Entities;
using PontoLegal.Repository.Interfaces;

namespace PontoLegal.Repository;

public class TimeClockNotificationRepository : ITimeClockNotificationRepository
{
    private readonly PontoLegalContext _context;

    public TimeClockNotificationRepository(PontoLegalContext context)
    {
        _context = context;
    }

    public async Task<bool> AddNotificationAsync(TimeClockNotification notification)
    {
        _context.TimeClockNotifications.Add(notification);
        return await _context.SaveChangesAsync() > 0;
    }
}
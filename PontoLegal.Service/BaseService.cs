using Flunt.Notifications;
using PontoLegal.Service.Interfaces;

namespace PontoLegal.Service;

public class BaseService : Notifiable<Notification>, IBaseService
{
    public List<string> GetNotifications()
    {
        var notifications = new List<string>();

        foreach (var notification in Notifications)
            notifications.Add(notification.Message);

        return notifications;
    }
}

using System.Collections.Generic;

namespace Atut.Services
{
    public interface INotificationManager
    {
        void Add(NotificationType type, string content);
        IEnumerable<NotificationEntry> GetAllAndClear();
        IEnumerable<NotificationEntry> GeByTypeAndClear(NotificationType type);
    }
}
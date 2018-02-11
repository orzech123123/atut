using System.Collections.Generic;
using System.Linq;

namespace Atut.Services
{
    public class NotificationManager : INotificationManager
    {
        private readonly IList<NotificationEntry> _entries = new List<NotificationEntry>();

        public void Add(NotificationType type, string content)
        {
            _entries.Add(new NotificationEntry
            {
                Type = type,
                Content = content
            });
        }

        public IEnumerable<NotificationEntry> GetAllAndClear()
        {
            var result = _entries.ToList();
            _entries.Clear();

            return result;
        }

        public IEnumerable<NotificationEntry> GeByTypeAndClear(NotificationType type)
        {
            var result = _entries.Where(entry => entry.Type == type).ToList();

            foreach (var notification in result)
            {
                _entries.Remove(notification);
            }

            return result;
        }
    }
}
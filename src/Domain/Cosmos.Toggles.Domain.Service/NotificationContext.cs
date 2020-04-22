using Cosmos.Toggles.Domain.DataTransferObject.Notifications;
using Cosmos.Toggles.Domain.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Service
{
    public class NotificationContext : INotificationContext
    {
        private readonly List<NotificationMessage> _notifications;
        public IReadOnlyCollection<NotificationMessage> Notifications => _notifications;
        public bool HasNotifications => _notifications.Any();

        public NotificationContext()
        {
            _notifications = new List<NotificationMessage> { };
        }

        public Task AddAsync(NotificationMessage notificationMessage)
        {
            _notifications.Add(notificationMessage);
            return Task.FromResult<object>(null);
        }

        public Task AddAsync(HttpStatusCode code, string description)
        {
            _notifications.Add(new NotificationMessage { Code = code, Description = description });
            return Task.FromResult<object>(null);
        }

        public Task AddAsync(HttpStatusCode code, string description, dynamic content)
        {
            _notifications.Add(new NotificationMessage { Code = code, Description = description, Content = content });
            return Task.FromResult<object>(null);
        }

        public bool Includes(HttpStatusCode code)
        {
            return this.HasNotifications && Notifications.Where(x => x.Code == code).Any();
        }
    }
}

using Cosmos.Toggles.Domain.DataTransferObject.Notifications;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Service.Interfaces
{
    public interface INotificationContext
    {
        IReadOnlyCollection<NotificationMessage> Notifications { get; }
        bool HasNotifications { get; }
        bool Includes(HttpStatusCode code);
        Task AddAsync(NotificationMessage notificationMessage);
        Task AddAsync(HttpStatusCode code, string description, string friendlyMessage = null);
    }
}

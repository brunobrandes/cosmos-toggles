using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace Cosmos.Toggles.Domain.DataTransferObject.Notifications
{
    public class NotificationMessage
    {
        public HttpStatusCode Code { get; set; }

        public string Description { get; set; }

        public dynamic Content { get; set; }

        public IEnumerable<string> FriendlyMessages { get; set; }

        [JsonIgnore]
        public NotificationLog Log { get; set; }
    }
}

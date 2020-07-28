using System;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class Token
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Jwt { get; set; }
        public DateTime Expires { get; set; }
        public int ExpiresMilliseconds
        {
            get
            {
                if (this.Expires != DateTime.MinValue)
                    return (int)(Expires - DateTime.UtcNow).TotalMilliseconds;

                return 0;
            }
        }
    }
}

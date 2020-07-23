using System;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class RefreshToken
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Jwt { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public string CreatedIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedIp { get; set; }
    }
}

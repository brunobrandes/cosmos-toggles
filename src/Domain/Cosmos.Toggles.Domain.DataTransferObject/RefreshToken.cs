using System;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class RefreshToken : Token
    {
        public DateTime Created { get; set; }
        public string CreatedIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedIp { get; set; }
    }
}

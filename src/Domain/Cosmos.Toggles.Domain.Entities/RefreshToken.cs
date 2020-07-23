using Cosmos.Db.Sql.Api.Domain.Entities;
using System;

namespace Cosmos.Toggles.Domain.Entities
{
    public class RefreshToken : Entity
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string Jwt { get; set; }
        public DateTime Created { get; set; }
        public string CreatedIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedIp { get; set; }
    }
}

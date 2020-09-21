using Cosmos.Db.Sql.Api.Domain.Entities;
using Cosmos.Toggles.Domain.Enum;
using System.Collections.Generic;

namespace Cosmos.Toggles.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; }
        public IEnumerable<string> Projects { get; set; }
    }
}

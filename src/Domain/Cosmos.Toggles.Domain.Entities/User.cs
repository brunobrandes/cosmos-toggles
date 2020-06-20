using Cosmos.Db.Sql.Api.Domain.Entities;

namespace Cosmos.Toggles.Domain.Entities
{
    public class User : Entity
    {
        public User() :
            base(false)
        {

        }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}

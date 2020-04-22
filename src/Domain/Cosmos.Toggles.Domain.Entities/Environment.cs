using Cosmos.Db.Sql.Api.Domain.Entities;

namespace Cosmos.Toggles.Domain.Entities
{
    public class Environment : Entity
    {
        public Environment() :
            base(false)
        {

        }
        public Project Project { get; set; }

        public string Name { get; set; }
    }
}

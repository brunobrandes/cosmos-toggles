using Cosmos.Db.Sql.Api.Domain.Entities;

namespace Cosmos.Toggles.Domain.Entities
{
    public class Project : Entity
    {
        public Project() :
            base(false)
        {

        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}

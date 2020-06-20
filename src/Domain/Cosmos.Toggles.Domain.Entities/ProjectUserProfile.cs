using Cosmos.Db.Sql.Api.Domain.Entities;

namespace Cosmos.Toggles.Domain.Entities
{
    public class ProjectUserProfile : Entity
    {
        public ProjectUserProfile() :
            base()
        {

        }
        public string ProjectId { get; set; }
        public FlagUserProfile FlagUserProfile { get; set; }
    }
}

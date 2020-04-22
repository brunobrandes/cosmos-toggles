using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Infra.Entities.Repositories;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Domain.Entities.Repositories;

namespace Cosmos.Toggles.Infra.Cosmos.Db
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(CosmosClient cosmosClient) : base(cosmosClient)
        {
        }

        public override string DatabaseId => "CosmosToggles";

        public override string ContainerId => "Projects";
    }
}

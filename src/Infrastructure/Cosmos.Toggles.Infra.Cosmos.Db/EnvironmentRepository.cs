using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Infra.Entities.Repositories;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Domain.Entities.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Infra.Cosmos.Db
{
    public class EnvironmentRepository : GenericRepository<Environment>, IEnvironmentRepository
    {
        private readonly CosmosContainer _flagContainer;

        public EnvironmentRepository(CosmosClient cosmosClient)
            : base(cosmosClient)
        {
            _flagContainer = cosmosClient.GetContainer(DatabaseId, ContainerId);
        }

        public override string DatabaseId => "CosmosToggles";

        public override string ContainerId => "Environments";

        public async Task<IEnumerable<Environment>> GetByProjectAsync(string projectId)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Environments C WHERE C.Project.id = @projectId").WithParameter("@projectId", projectId);
            var pageable = _flagContainer.GetItemQueryIterator<Environment>(queryDefinition, null, new QueryRequestOptions { PartitionKey = new PartitionKey(projectId) });
            var result = new List<Environment> { };

            await foreach (var page in pageable?.AsPages())
            {
                foreach (var entity in page?.Values)
                {
                    result.Add(entity);
                }
            }

            return result;
        }
    }
}

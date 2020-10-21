using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Infra.Entities.Repositories;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Domain.Entities.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Infra.Cosmos.Db
{
    public class FlagRepository : GenericRepository<Flag>, IFlagRepository
    {
        private readonly CosmosContainer _flagContainer;

        public FlagRepository(CosmosClient cosmosClient) : base(cosmosClient)
        {
            _flagContainer = cosmosClient.GetContainer(DatabaseId, ContainerId);
        }

        public override string DatabaseId => "CosmosToggles";

        public override string ContainerId => "Flags";

        public async Task<Flag> GetAsync(string projectId, string environmentId, string flagId)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Flags C WHERE C.Environment.Project.id = @projectId AND C.Environment.id = @environmentId AND C.id = @flagId")
                .WithParameter("@projectId", projectId)
                .WithParameter("@environmentId", environmentId)
                .WithParameter("@flagId", flagId);

            var pageable = _flagContainer.GetItemQueryIterator<Flag>(queryDefinition, null, new QueryRequestOptions { PartitionKey = new PartitionKey(environmentId) });

            await foreach (var page in pageable?.AsPages())
            {
                return page?.Values.FirstOrDefault();
            }

            return null;
        }

        public async Task<IEnumerable<Flag>> GetAsync(string projectId, string environmentId)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Flags C WHERE C.Environment.Project.id = @projectId AND C.Environment.id = @environmentId")
                .WithParameter("@projectId", projectId)
                .WithParameter("@environmentId", environmentId);

            var pageable = _flagContainer.GetItemQueryIterator<Flag>(queryDefinition, null, new QueryRequestOptions { PartitionKey = new PartitionKey(environmentId) });

            await foreach (var page in pageable?.AsPages())
            {
                return page?.Values;
            }

            return null;
        }
    }
}

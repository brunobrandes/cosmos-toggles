using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Infra.Entities.Repositories;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Domain.Entities.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Cosmos.Toggles.Domain.Service.Extensions;

namespace Cosmos.Toggles.Infra.Cosmos.Db
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly CosmosContainer _refreshTokenContainer;

        public RefreshTokenRepository(CosmosClient cosmosClient) : base(cosmosClient)
        {
            _refreshTokenContainer = cosmosClient.GetContainer(DatabaseId, ContainerId);
        }

        public override string DatabaseId => "CosmosToggles";

        public override string ContainerId => "RefreshTokens";

        public async Task<RefreshToken> GetByKeyUserIdAsync(string key, string userId)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM RefreshTokens C WHERE C.Key = @key")
               .WithParameter("@key", key);

            var pageable = _refreshTokenContainer.GetItemQueryIterator<RefreshToken>(queryDefinition, null, new QueryRequestOptions { PartitionKey = new PartitionKey(userId) });

            await foreach (var page in pageable?.AsPages())
            {
                return page?.Values.FirstOrDefault();
            }

            return null;
        }

        public async Task TryUpdateAsync(RefreshToken refreshTokenEntity, string userId)
        {
            try
            {
               await this.UpdateAsync(refreshTokenEntity, new PartitionKey(userId));
            }
            catch (Exception ex)
            {
                ex.IgnoreCosmosExceptionStatus(HttpStatusCode.NotFound);    
            }
        }
    }
}

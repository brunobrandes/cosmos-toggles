using Azure.Cosmos;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Entities.Repositories;

namespace Cosmos.Toggles.Infra.Cosmos.Db
{
    public class CosmosToggleDataContext : ICosmosToggleDataContext
    {
        public CosmosToggleDataContext(CosmosClient cosmosClient)
        {
            EnvironmentRepository = new EnvironmentRepository(cosmosClient);
            FlagRepository = new FlagRepository(cosmosClient);
            ProjectRepository = new ProjectRepository(cosmosClient);
            UserRepository = new UserRepository(cosmosClient);
            RefreshTokenRepository = new RefreshTokenRepository(cosmosClient);
        }

        public IEnvironmentRepository EnvironmentRepository { get; set; }
        public IFlagRepository FlagRepository { get; set; }
        public IProjectRepository ProjectRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; set; }
    }
}

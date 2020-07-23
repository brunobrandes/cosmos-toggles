using Cosmos.Toggles.Domain.Entities.Repositories;

namespace Cosmos.Toggles.Domain.Entities.Interfaces
{
    public interface ICosmosToggleDataContext
    {
        public IEnvironmentRepository EnvironmentRepository { get; set; }
        public IFlagRepository FlagRepository { get; set; }
        public IProjectRepository ProjectRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; set; }
    }
}

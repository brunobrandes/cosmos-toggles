using Azure.Cosmos;
using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Entities.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetByKeyUserIdAsync(string key, string userId);
        Task TryUpdateAsync(RefreshToken refreshTokenEntity, string userId);
    }
}

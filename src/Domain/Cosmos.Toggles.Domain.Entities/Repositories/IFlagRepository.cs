using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Entities.Repositories
{
    public interface IFlagRepository : IGenericRepository<Flag>
    {
        Task<Flag> GetByEnvironmentAsync(string projectId, string environmentId, string flagId);
    }
}

using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Entities.Repositories
{
    public interface IFlagRepository : IGenericRepository<Flag>
    {
        Task<Flag> GetAsync(string projectId, string environmentId, string flagId);
        Task<IEnumerable<Flag>> GetAsync(string projectId, string environmentId);
    }
}

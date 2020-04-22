using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Entities.Repositories
{
    public interface IEnvironmentRepository : IGenericRepository<Environment>
    {
        Task<System.Collections.Generic.IEnumerable<Environment>> GetByProjectAsync(string projectId);
    }
}

using Cosmos.Db.Sql.Api.Domain.Entities.Repositories;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Entities.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByEmailPasswordAsync(string email, string password);
    }
}

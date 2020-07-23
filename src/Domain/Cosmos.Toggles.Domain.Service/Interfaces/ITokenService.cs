using Cosmos.Toggles.Domain.DataTransferObject;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Service.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateJwtAsync(string userId, string userName, string userEmail, int expiresSeconds);

        Task<string> CreateKeyAsync();

        Task<User> ExtractUserAsync(string jwt);
    }
}

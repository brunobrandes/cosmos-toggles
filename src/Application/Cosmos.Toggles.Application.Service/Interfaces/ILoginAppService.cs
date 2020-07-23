using Cosmos.Toggles.Domain.DataTransferObject;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface ILoginAppService
    {
        Task<RefreshToken> LoginAsync(Login login, string ipAddress);
        Task<RefreshToken> RefreshAsync(string key, string userId, string ipAddress);
    }
}

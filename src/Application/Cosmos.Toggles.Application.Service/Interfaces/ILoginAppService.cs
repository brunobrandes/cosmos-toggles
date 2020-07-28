using Cosmos.Toggles.Domain.DataTransferObject;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface ILoginAppService
    {
        Task<Token> LoginAsync(Login login, string ipAddress);
        Task<Token> RefreshAsync(string key, string userId, string ipAddress);
    }
}

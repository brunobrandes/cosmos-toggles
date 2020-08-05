using Cosmos.Toggles.Domain.DataTransferObject;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Service.Interfaces
{
    public interface ISecurityContext
    {
        Task<User> GetUserAsync();
    }
}

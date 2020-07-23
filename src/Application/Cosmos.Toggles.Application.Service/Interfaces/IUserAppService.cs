using Cosmos.Toggles.Domain.DataTransferObject;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface IUserAppService 
    {
        Task CreateAsync(User user);
    }
}

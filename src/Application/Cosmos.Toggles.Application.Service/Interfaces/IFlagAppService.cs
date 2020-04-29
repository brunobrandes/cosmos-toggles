using Cosmos.Toggles.Domain.DataTransferObject;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface IFlagAppService
    {
        Task CreateAsync(Flag flag);
        Task<Flag> GetAsync(string projectId, string environmentId, string key);
        Task<FlagStatus> GetStatusAsync(string projectId, string environmentId, string flagId);
        Task<int> UpdateAsync(Flag flag);     
    }
}

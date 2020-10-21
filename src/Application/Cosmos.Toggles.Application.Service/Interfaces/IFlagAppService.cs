using Cosmos.Toggles.Domain.DataTransferObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface IFlagAppService
    {
        Task CreateAsync(Flag flag);
        Task<Flag> GetAsync(string projectId, string environmentId, string flagId);
        Task<IEnumerable<Flag>> GetAsync(string projectId, string environmentId);
        Task<FlagStatus> GetStatusAsync(string projectId, string environmentId, string flagId);
        Task<int> UpdateAsync(Flag flag);     
    }
}

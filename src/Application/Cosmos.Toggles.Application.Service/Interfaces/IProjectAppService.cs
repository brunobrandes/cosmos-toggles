using Cosmos.Toggles.Domain.DataTransferObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface IProjectAppService
    {
        Task CreateAsync(Project project);
        Task<Project> GetAsync(string id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByUserIdAsync(string userId);
    }
}

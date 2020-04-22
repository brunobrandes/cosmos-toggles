using Cosmos.Toggles.Domain.DataTransferObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface IEnvironmentAppService
    {
        Task CreateAsync(Environment environment);
        Task<IEnumerable<Environment>> GetByProjectAsync(string key);
    }
}

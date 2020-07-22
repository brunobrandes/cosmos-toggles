using Cosmos.Toggles.Domain.DataTransferObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface IBaseAppService<T> where T : class
    {
        Task CreateAsync(T dto);
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}

using Cosmos.Toggles.Domain.DataTransferObject;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service.Interfaces
{
    public interface IFeatureFlagAppService
    {
        Task<FeatureFlag> GetByEnvironmentAsync(string projectId, string environmentId, string flagId);
    }
}

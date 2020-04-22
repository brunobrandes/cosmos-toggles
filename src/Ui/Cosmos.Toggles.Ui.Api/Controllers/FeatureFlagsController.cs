using Cosmos.Toggles.Application.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    /// <summary>
    /// Consult feature flags
    /// </summary>
    [Route("feature-flags")]
    [ApiController]
    public class FeatureFlagsController : ControllerBase
    {
        /// <summary>
        /// Check feature flags status by key and environment key
        /// </summary>
        /// <param name="featureFlagAppService"></param>
        /// <param name="projectKey"></param>
        /// <param name="key"></param>
        /// <param name="environmentKey"></param>
        /// <returns></returns>
        [HttpGet("{projectId}/{environmentId}/{flagId}/status")]
        public async Task<IActionResult> GetFeatureFlagStatusAsync([FromServices] IFeatureFlagAppService featureFlagAppService,
             string projectId, string environmentId, string flagId)
        {
            return Ok(await featureFlagAppService.GetByEnvironmentAsync(projectId, environmentId, flagId));
        }
    }
}
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    /// <summary>
    /// Flag controller
    /// </summary>
    [Authorize("Bearer")]
    [ApiController]
    [Route("flags")]
    public class FlagsController : ControllerBase
    {
        /// <summary>
        /// Create flag
        /// </summary>
        /// <param name="flagAppService">Instance of flag app service</param>
        /// <param name="flag">Flag</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromServices] IFlagAppService flagAppService, [FromBody] Flag flag)
        {
            await flagAppService.CreateAsync(flag);
            return Created($"{Request.Path}", flag);
        }

        /// <summary>
        /// Create flag
        /// </summary>
        /// <param name="flagAppService">Instance of flag app service</param>
        /// <param name="environmentAppService">Instance of environment app service</param>
        /// <param name="environmentId">Environment identifier</param>
        /// <param name="flag">Flag</param>
        /// <returns></returns>
        [HttpPost("{projectId}/{environmentId}/")]
        public async Task<IActionResult> CreateAsync([FromServices] IFlagAppService flagAppService,
            [FromServices] IEnvironmentAppService environmentAppService, string projectId, string environmentId, [FromBody] Flag flag)
        {
            flag.Environment = await environmentAppService.GetAsync(projectId, environmentId);
            await flagAppService.CreateAsync(flag);

            return Ok();
        }

        /// <summary>
        /// Get flag by project and environment identifier
        /// </summary>
        /// <param name="flagAppService">Instance of flag app service</param>
        /// <param name="projectId">Project identifier</param>
        /// <param name="environmentId">Environment identifier</param>
        /// <returns>Flag list</returns>
        [HttpGet("{projectId}/{environmentId}/")]
        public async Task<IActionResult> GetByEnviromentAsync([FromServices] IFlagAppService flagAppService,
             string projectId, string environmentId)
        {
            return Ok(await flagAppService.GetAsync(projectId, environmentId));
        }

        /// <summary>
        /// Get flag by project, environment and flag identifier
        /// </summary>
        /// <param name="flagAppService">Instance of flag app service</param>
        /// <param name="projectId">Project identifier</param>
        /// <param name="environmentId">Environment identifier</param>
        /// <param name="flagId">Flag identifier</param>
        /// <returns>Flag</returns>
        [HttpGet("{projectId}/{environmentId}/{flagId}")]
        public async Task<IActionResult> GetByEnviromentAsync([FromServices] IFlagAppService flagAppService,
             string projectId, string environmentId, string flagId)
        {
            return Ok(await flagAppService.GetAsync(projectId, environmentId, flagId));
        }

        /// <summary>
        /// Get flag status
        /// </summary>
        /// <param name="flagAppService">Instance of flag app service</param>
        /// <param name="projectId">Project identifier</param>
        /// <param name="environmentId">Environment identifier</param>
        /// <param name="flagId">Flag identifier</param>
        /// <returns>FlagStatus</returns>
        [HttpGet("{projectId}/{environmentId}/{flagId}/status")]
        public async Task<IActionResult> GetFeatureFlagStatusAsync([FromServices] IFlagAppService flagAppService,
          string projectId, string environmentId, string flagId)
        {
            return Ok(await flagAppService.GetStatusAsync(projectId, environmentId, flagId));
        }

        /// <summary>
        /// Edit flag
        /// </summary>
        /// <param name="flagAppService">Instance of flag app service</param>
        /// <param name="flag">Flag</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutAsync([FromServices] IFlagAppService flagAppService, [FromBody] Flag flag)
        {
            await flagAppService.UpdateAsync(flag);
            return Ok(flag);
        }
    }
}

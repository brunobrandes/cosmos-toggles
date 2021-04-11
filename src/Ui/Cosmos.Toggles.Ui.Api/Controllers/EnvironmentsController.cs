using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    /// <summary>
    /// Manage environment data
    /// </summary>
    [Authorize("Bearer")]
    [ApiController]
    [Route("environments")]
    public class EnvironmentsController : ControllerBase
    {
        /// <summary>
        /// Create evironment
        /// </summary>
        /// <param name="environmentAppService"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromServices] IEnvironmentAppService environmentAppService, [FromBody] Environment environment)
        {
            await environmentAppService.CreateAsync(environment);
            return Created($"{Request.Path}", environment);
        }

        /// <summary>
        /// Create evironment
        /// </summary>
        /// <param name="environmentAppService">Istance of environment app service</param>
        /// <param name="projectAppService">Istance of project app service</param>
        /// <param name="projectId">Project identifier</param>
        /// <param name="environment">Environment</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{projectId}")]
        public async Task<IActionResult> AddAsync([FromServices] IEnvironmentAppService environmentAppService,
            [FromServices] IProjectAppService projectAppService, string projectId, [FromBody] Environment environment)
        {
            environment.Project = await projectAppService.GetAsync(projectId);
            await environmentAppService.CreateAsync(environment);
            return Created($"{Request.Path}", environment);
        }
    }
}

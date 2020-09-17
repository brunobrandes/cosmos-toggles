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
        /// Get environment by project id
        /// </summary>
        /// <param name="environmentAppService"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetAsync([FromServices] IEnvironmentAppService environmentAppService, string projectId)
        {
            return Ok(await environmentAppService.GetByProjectAsync(projectId));
        }        
    }
}

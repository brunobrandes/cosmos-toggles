using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectsController : ControllerBase
    {
        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="projectAppService"></param>
        /// <param name="project">Project</param>
        /// <returns>Project</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromServices] IProjectAppService projectAppService, [FromBody] Project project)
        {
            await projectAppService.CreateAsync(project);
            return Created($"{Request.Path}", project);
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <param name="projectAppService"></param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync([FromServices] IProjectAppService projectAppService)
        {
            return Ok(await projectAppService.GetAllAsync());
        }

        /// <summary>
        /// Get project by identifier
        /// </summary>
        /// <param name="projectAppService"></param>
        /// <param name="projectId">Project identifier</param>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetAsync([FromServices] IProjectAppService projectAppService, string projectId)
        {
            return Ok(await projectAppService.GetAsync(projectId));
        }       
    }
}

using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    /// <summary>
    /// User controller
    /// </summary>
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="userAppService">User app service</param>
        /// <param name="user">User</param>
        /// <returns>User</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromServices] IUserAppService userAppService, [FromBody] User user)
        {
            await userAppService.CreateAsync(user);
            return Created($"{Request.Path}", user);
        }

        /// <summary>
        /// Get projects by user
        /// </summary>
        /// <param name="projectAppService">Project app service</param>.
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        [HttpGet("{userId}/projects")]
        public async Task<IActionResult> GetAsync([FromServices] IProjectAppService projectAppService, string userId)
        {
            var projects = await projectAppService.GetByUserIdAsync(userId);
            return Ok(projects);
        }

        /// <summary>
        /// Add project to user
        /// </summary>
        /// <param name="userAppService">User app service</param>
        /// <param name="userId">User identifier</param>
        /// <param name="projectId">Project identifier</param>
        /// <returns></returns>
        [HttpPut("{userId}/projects/{projectId}/add")]
        public async Task<IActionResult> GetAsync([FromServices] IUserAppService userAppService, string userId, string projectId)
        {
            await userAppService.AddProjectAsync(userId, projectId);
            return Ok();
        }
    }
}

using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="userAppService">User app service</param>
        /// <param name="user">User</param>
        /// <returns>User</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromServices] IUserAppService userAppService, [FromBody] User user)
        {
            await userAppService.CreateAsync(user);
            return Created($"{Request.Path}", user);
        }
    }
}

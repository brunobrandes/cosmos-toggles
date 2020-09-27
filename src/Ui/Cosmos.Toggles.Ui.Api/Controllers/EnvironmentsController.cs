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
    }
}

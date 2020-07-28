using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Ui.Api.Controllers
{
    /// <summary>
    /// Authentication
    /// </summary>
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginAppService">Instance of service</param>
        /// <param name="login">Login</param>
        /// <returns>RefreshToken</returns>
        [HttpPost("")]
        public async Task<IActionResult> LoginAsync([FromServices] ILoginAppService loginAppService, [FromForm] Login login)
        {
            var refreshToken = await loginAppService.LoginAsync(login, Request.HttpContext.Connection.RemoteIpAddress.AddressFamily.ToString());
            return Ok(refreshToken);
        }

        /// <summary>
        /// Refresh login
        /// </summary>
        /// <param name="loginAppService">Instance of service</param>
        /// <param name="loginRefresh">Login refresh</param>
        /// <returns>RefreshToken</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAsync([FromServices] ILoginAppService loginAppService, [FromForm] LoginRefresh loginRefresh)
        {
            var token = await loginAppService.RefreshAsync(loginRefresh.Key, loginRefresh.UId,
                Request.HttpContext.Connection.RemoteIpAddress.AddressFamily.ToString());

            return Ok(token);
        }
    }
}

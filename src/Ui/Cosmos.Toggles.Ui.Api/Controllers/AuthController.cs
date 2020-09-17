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
        /// <param name="authAppService">Instance of service</param>
        /// <param name="login">Login</param>
        /// <returns>RefreshToken</returns>
        [HttpPost("")]
        public async Task<IActionResult> LoginAsync([FromServices] IAuthAppService authAppService, [FromForm] Login login)
        {
            var refreshToken = await authAppService.LoginAsync(login, Request.HttpContext.Connection.RemoteIpAddress.AddressFamily.ToString());
            return Ok(refreshToken);
        }

        /// <summary>
        /// Refresh token
        /// </summary>
        /// <param name="authAppService">Instance of service</param>
        /// <param name="loginRefresh">Login refresh</param>
        /// <returns>RefreshToken</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshAsync([FromServices] IAuthAppService authAppService, [FromForm] LoginRefresh loginRefresh)
        {
            var token = await authAppService.RefreshAsync(loginRefresh.Key, loginRefresh.UId,
                Request.HttpContext.Connection.RemoteIpAddress.AddressFamily.ToString());

            return Ok(token);
        }
    }
}

using Cosmos.Toggles.Application.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Infra.Http.Filters
{
    public class UserAuthProjectIdParamFilter : IAsyncActionFilter
    {
        private const string PARAMETER_NAME = "projectId";

        private readonly IAuthAppService _authAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAuthProjectIdParamFilter(IHttpContextAccessor httpContextAccessor, IAuthAppService authAppService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authAppService = authAppService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.RouteData.Values.ContainsKey(PARAMETER_NAME))
                throw new ArgumentNullException(PARAMETER_NAME);

            var projectId = (context.RouteData.Values[PARAMETER_NAME] as string);
            await _authAppService.UserHasAuthProjectAsync(projectId);
        }
    }
}

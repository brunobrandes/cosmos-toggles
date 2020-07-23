using Cosmos.Toggles.Application.Service;
using Cosmos.Toggles.Application.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cosmos.Toggles.Infra.DependencyInjection
{
    public static class AppServiceConfig
    {
        public static void AddCosmosToggleAppServices(this IServiceCollection services)
        {
            services
                .AddScoped<IEnvironmentAppService, EnvironmentAppService>()
                .AddScoped<IFlagAppService, FlagAppService>()
                .AddScoped<ILoginAppService, LoginAppService>()
                .AddScoped<IProjectAppService, ProjectAppService>()
                .AddScoped<IUserAppService, UserAppService>();            
        }
    }
}

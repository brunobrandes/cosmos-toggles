using Cosmos.Toggles.Domain.Service;
using Cosmos.Toggles.Domain.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Cosmos.Toggles.Infra.DependencyInjection
{
    public static class ServiceConfig
    {
        public static void AddCosmosToggleDomainServices(this IServiceCollection services)
        {
            services
                .AddScoped<ITokenService, JwtTokenService>();
        }
    }
}

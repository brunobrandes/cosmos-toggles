using Cosmos.Toggles.Domain.Service;
using Cosmos.Toggles.Domain.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Cosmos.Toggles.Ui.Api.Extensions
{
    /// <summary>
    /// Web host builder extension
    /// </summary>
    public static class WebHostBuilderExtension
    {
        public static IWebHostBuilder ConfigureTokenService(this IWebHostBuilder builder)
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ITokenService, JwtTokenService>();
            });
        }
    }
}

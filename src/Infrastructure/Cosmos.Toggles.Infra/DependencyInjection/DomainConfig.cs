using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.DataTransferObject.Validators;
using Cosmos.Toggles.Domain.Service;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cosmos.Toggles.Infra.DependencyInjection
{
    public static class DomainConfig
    {
        public static void AddCosmosToggleDTOValidators(this IServiceCollection services)
        {
            services
              .AddScoped<IValidator<Environment>, EnvironmentValidator>()
              .AddScoped<IValidator<Flag>, FlagValidator>()
              .AddScoped<IValidator<Project>, ProjectValidator>();
        }

        public static void AddCosmosToggleNotificationContext(this IServiceCollection services)
        {
            services
              .AddScoped<INotificationContext, NotificationContext>();
        }
    }
}

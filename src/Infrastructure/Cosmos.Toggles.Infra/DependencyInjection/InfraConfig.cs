using Azure.Cosmos;
using Azure.Cosmos.Serialization;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Interfaces;
using Cosmos.Toggles.Infra.Cosmos.Db;
using Cosmos.Toggles.Infra.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Cosmos.Toggles.Infra.DependencyInjection
{
    public static class InfraConfig
    {
        public static void AddCosmosToggleDataContext(this IServiceCollection services, string connectionString)
        {
            services
                .AddScoped<ICosmosToggleDataContext>(x => new CosmosToggleDataContext(new CosmosClient(connectionString,
                new CosmosClientOptions
                {
                    ConnectionMode = ConnectionMode.Direct,
                    SerializerOptions = new CosmosSerializationOptions { Indented = true, PropertyNamingPolicy = CosmosPropertyNamingPolicy.Default }
                })));

            services.AddScoped<ISecurityContext, HttpSecurityContext>();
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cosmos.Toggles.Domain.Enum
{
    public static class CosmosSerializerSettings
    {
        public static JsonSerializerSettings NewtonsoftJsonSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                };
            }
        }
    }
}

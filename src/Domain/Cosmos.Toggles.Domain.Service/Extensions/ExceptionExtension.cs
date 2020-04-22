using Azure.Cosmos;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Enum;
using System;

namespace Cosmos.Toggles.Domain.Service.Extensions
{
    public static class ExceptionExtension
    {
        public const int GENERIC_ERROR = 500;
        public static FeatureFlag ToFeatureFlag(this Exception exception, string flagId)
        {
            var result = new FeatureFlag
            {
                Id = flagId,
                Description = exception.Message,
                Code = GENERIC_ERROR,
                Status = FeatureFlagStatus.Unavailable
            };

            if (exception is CosmosException)
            {
                var cosmosException = exception as CosmosException;
                result.Code = cosmosException.Status;
            }

            return result;
        }
    }
}

﻿using Azure.Cosmos;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Enum;
using System;
using System.Net;

namespace Cosmos.Toggles.Domain.Service.Extensions
{
    public static class ExceptionExtension
    {
        public const int GENERIC_ERROR = 500;
        public static FlagStatus ToFlagStatus(this Exception exception, string flagId)
        {
            var result = new FlagStatus
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

        public static void IgnoreCosmosExceptionStatus(this Exception exception, HttpStatusCode httpStatusCode)
        {
            if (exception is CosmosException)
            {
                var cosmosException = exception as CosmosException;
                if (cosmosException.Status != (int)httpStatusCode)
                {
                    throw exception;
                }
            }
            else
            {
                throw exception;
            }
        }
    }
}

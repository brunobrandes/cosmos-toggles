using Azure.Cosmos;
using Cosmos.Toggles.Domain.DataTransferObject.Notifications;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Infra.Http.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await WriteResponseAsync(context, ex);
            }
        }

        private async Task WriteResponseAsync(HttpContext context, Exception ex)
        {
            var notificationMessages = new List<NotificationMessage> { };
            var code = HttpStatusCode.InternalServerError;
            var description = ex.Message;
            var friendlyMessage = string.Empty;

            if (ex is CosmosException)
            {
                var cosmosException = ex as CosmosException;

                switch (cosmosException.Status)
                {
                    case 409:
                        code = HttpStatusCode.Conflict;
                        description = "Already exists";
                        break;

                    case 404:
                        code = HttpStatusCode.NotFound;
                        description = "Nout found";
                        break;
                }
            }

            if (ex is ValidationException)
            {
                code = HttpStatusCode.BadRequest;
                friendlyMessage = ex.Message;
            }

            context.Response.StatusCode = (int)code;
            notificationMessages.Add(new NotificationMessage
            {
                Code = code,
                Description = description,
                FriendlyMessages = new List<string> { friendlyMessage }
            });

            var responseText = JsonConvert.SerializeObject(notificationMessages, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            });

            await context.Response.WriteAsync(responseText);
        }
    }
}


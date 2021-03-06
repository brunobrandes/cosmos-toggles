﻿using AutoMapper;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Enum;
using Cosmos.Toggles.Domain.Service.Extensions;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service
{
    public class FlagAppService : IFlagAppService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Flag> _flagValidator;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly INotificationContext _notificationContext;
        private readonly IAuthAppService _authAppService;

        public FlagAppService(IMapper mapper, IValidator<Flag> flagValidator, ICosmosToggleDataContext cosmosToggleDataContext,
            INotificationContext notificationContext, IAuthAppService authAppService)
        {
            _mapper = mapper;
            _flagValidator = flagValidator;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
            _authAppService = authAppService;
        }

        public async Task CreateAsync(Flag flag)
        {
            _flagValidator.ValidateAndThrow(flag);

            if (await _authAppService.UserHasAuthProjectAsync(flag.Environment.Project.Id))
            {
                var entity = _mapper.Map<Domain.Entities.Flag>(flag);
                await _cosmosToggleDataContext.FlagRepository.AddAsync(entity, flag.Environment.Id);
            }
        }

        public async Task<Flag> GetAsync(string projectId, string environmentId, string flagId)
        {
            if (!await _authAppService.UserHasAuthProjectAsync(projectId))
                return null;

            var entity = await _cosmosToggleDataContext.FlagRepository.GetAsync(projectId, environmentId, flagId);

            if (entity == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Flag not found - Id: '{flagId}' - EnviromentId: '{environmentId}' - ProjectId: {projectId}");
                return null;
            }

            return _mapper.Map<Flag>(entity);
        }

        public async Task<IEnumerable<Flag>> GetAsync(string projectId, string environmentId)
        {
            if (!await _authAppService.UserHasAuthProjectAsync(projectId))
                return null;

            var entities = await _cosmosToggleDataContext.FlagRepository.GetAsync(projectId, environmentId);

            if (entities == null || entities.Count() == 0)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Flag not found | EnviromentId: '{environmentId}' - ProjectId: {projectId}");
                return null;
            }

            return _mapper.Map<IEnumerable<Flag>>(entities);
        }

        public async Task<FlagStatus> GetStatusAsync(string projectId, string environmentId, string flagId)
        {
            if (!await _authAppService.UserHasAuthProjectAsync(projectId))
                return null;

            try
            {
                var entity = await _cosmosToggleDataContext.FlagRepository.GetAsync(projectId, environmentId, flagId);
                var code = (int)HttpStatusCode.OK;

                if (entity != null)
                {
                    return _mapper.Map<FlagStatus>(entity, opts =>
                    {
                        opts.Items["Code"] = code;
                        opts.Items["Description"] = $"Feature flag query successfully";
                    });
                }

                return new FlagStatus
                {
                    Code = code,
                    Description = "Feature flag not found",
                    Status = FeatureFlagStatus.Unavailable
                };
            }
            catch (Exception ex)
            {
                return ex.ToFlagStatus(flagId);
            }
        }

        /// <summary>
        /// Update flag
        /// </summary>
        /// <param name="flag">Flag</param>
        /// <returns>Return 'rows' afected</returns>
        public async Task<int> UpdateAsync(Flag flag)
        {
            _flagValidator.ValidateAndThrow(flag);

            if (!await _authAppService.UserHasAuthProjectAsync(flag.Environment.Project.Id))
                return 0;

            var entity = await _cosmosToggleDataContext.FlagRepository.GetAsync(flag.Environment.Project.Id, flag.Environment.Id, flag.Id);

            if (entity != null)
            {
                entity = _mapper.Map<Domain.Entities.Flag>(flag);
                await _cosmosToggleDataContext.FlagRepository.UpdateAsync(entity, flag.Environment.Id);
                return 1;
            }

            await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Flag not found. Project: {flag.Environment.Project.Id} - Enviroment: {flag.Environment.Id} - Id: {flag.Id}");
            return 0;
        }
    }
}

using AutoMapper;
using Azure.Cosmos;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service
{
    public class EnvironmentAppService : IEnvironmentAppService
    {
        private readonly IMapper _mapper;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly IValidator<Environment> _environmentValidator;
        private readonly INotificationContext _notificationContext;
        private readonly IAuthAppService _authAppService;

        public EnvironmentAppService(IMapper mapper, IValidator<Environment> environmentValidator,
            ICosmosToggleDataContext cosmosToggleDataContext, INotificationContext notificationContext, IAuthAppService authAppService)
        {
            _mapper = mapper;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _environmentValidator = environmentValidator;
            _notificationContext = notificationContext;
            _authAppService = authAppService;
        }

        public async Task CreateAsync(Environment environment)
        {
            _environmentValidator.ValidateAndThrow(environment, ruleSet: "Create");

            if (await _authAppService.UserHasAuthProjectAsync(environment.Project.Id))
            {
                var entity = _mapper.Map<Domain.Entities.Environment>(environment);
                await _cosmosToggleDataContext.EnvironmentRepository.AddAsync(entity, new PartitionKey(environment.Project.Id));
            }
        }

        public async Task<IEnumerable<Environment>> GetByProjectAsync(string projectId)
        {
            if (!await _authAppService.UserHasAuthProjectAsync(projectId))
                return null;

            var entities = await _cosmosToggleDataContext.EnvironmentRepository.GetByProjectAsync(projectId);

            if (entities == null || !entities.Any())
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Environments not found by project id '{projectId}'");
                return null;
            }

            return _mapper.Map<IEnumerable<Environment>>(entities);

        }
    }
}

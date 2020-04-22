using AutoMapper;
using Azure.Cosmos;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
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

        public FlagAppService(IMapper mapper, IValidator<Flag> flagValidator, ICosmosToggleDataContext cosmosToggleDataContext, INotificationContext notificationContext)
        {
            _mapper = mapper;
            _flagValidator = flagValidator;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
        }

        public async Task CreateAsync(Flag flag)
        {
            _flagValidator.Validate(flag, ruleSet: "CreateOrUpdate");
            var entity = _mapper.Map<Domain.Entities.Flag>(flag);
            await _cosmosToggleDataContext.FlagRepository.AddAsync(entity, new PartitionKey(flag.Environment.Id));
        }

        public async Task<Flag> GetAsync(string projectId, string environmentId, string flagId)
        {
            var entity = await _cosmosToggleDataContext.FlagRepository.GetByEnvironmentAsync(projectId, environmentId, flagId);

            if (entity == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Flag not found - Id: '{flagId}' - EnviromentId: '{environmentId}' - ProjectId: {projectId}");
                return null;
            }

            return _mapper.Map<Flag>(entity);
        }

        /// <summary>
        /// Update flag
        /// </summary>
        /// <param name="flag">Flag</param>
        /// <returns>Return 'rows' afected</returns>
        public async Task<int> UpdateAsync(Flag flag)
        {
            _flagValidator.Validate(flag, ruleSet: "CreateOrUpdate");

            var entity = await _cosmosToggleDataContext.FlagRepository.GetByEnvironmentAsync(flag.Environment.Project.Id, flag.Environment.Id, flag.Id);

            if (entity != null)
            {
                entity = _mapper.Map<Domain.Entities.Flag>(flag);
                await _cosmosToggleDataContext.FlagRepository.UpdateAsync(entity, new PartitionKey(flag.Environment.Id));
                return 1;
            }

            await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Flag not found. Project: {flag.Environment.Project.Id} - Enviroment: {flag.Environment.Id} - Id: {flag.Id}");
            return 0;
        }
    }
}

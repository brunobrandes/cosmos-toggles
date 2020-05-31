using AutoMapper;
using Azure.Cosmos;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service
{
    public class ProjectAppService : IProjectAppService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Project> _productValidator;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly INotificationContext _notificationContext;

        public ProjectAppService(IMapper mapper, IValidator<Project> productValidator, ICosmosToggleDataContext cosmosToggleDataContext,
            INotificationContext notificationContext)
        {
            _mapper = mapper;
            _productValidator = productValidator;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
        }

        public async Task CreateAsync(Project project)
        {
            _productValidator.ValidateAndThrow(project, ruleSet: "Create");
            var entity = _mapper.Map<Domain.Entities.Project>(project);
            await _cosmosToggleDataContext.ProjectRepository.AddAsync(entity, new PartitionKey(project.Id));
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            var result = new List<Project> { };

            await foreach (var entity in _cosmosToggleDataContext.ProjectRepository.GetAllAsync())
            {
                result.Add(_mapper.Map<Project>(entity));
            }

            if (result.Count == 0)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Projects not found");
                return null;
            }

            return result;
        }

        public async Task<Project> GetAsync(string projectId)
        {
            var entity = await _cosmosToggleDataContext.ProjectRepository.GetByIdAsync(projectId, new PartitionKey(projectId));

            if (entity == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Project not found by id '{projectId}'");
                return null;
            }

            return _mapper.Map<Project>(entity);
        }
    }
}

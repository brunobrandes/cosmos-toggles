using AutoMapper;
using Azure.Cosmos;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUserAppService _userAppService;
        private readonly ISecurityContext _securityContrext;

        public ProjectAppService(IMapper mapper, IValidator<Project> productValidator, ICosmosToggleDataContext cosmosToggleDataContext,
            INotificationContext notificationContext, IUserAppService userAppService, ISecurityContext securityContrext)
        {
            _mapper = mapper;
            _productValidator = productValidator;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
            _userAppService = userAppService;
            _securityContrext = securityContrext;
        }

        public async Task CreateAsync(Project project)
        {
            _productValidator.ValidateAndThrow(project, ruleSet: "Create");
            var entity = _mapper.Map<Domain.Entities.Project>(project);
            await _cosmosToggleDataContext.ProjectRepository.AddAsync(entity, new PartitionKey(entity.Id));

            var user = await _securityContrext.GetUserAsync();

            try
            {
                await _userAppService.AddProjectAsync(user, project.Id);
            }
            catch 
            {
                await _cosmosToggleDataContext.ProjectRepository.DeleteAsync(entity.Id, new PartitionKey(entity.Id));
                throw;
            }
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

        public async Task<Project> GetAsync(string id)
        {
            var entity = await _cosmosToggleDataContext.ProjectRepository.GetByIdAsync(id, new PartitionKey(id));

            if (entity == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Project not found by id '{id}'");
                return null;
            }

            return _mapper.Map<Project>(entity);
        }

        public async Task<IEnumerable<Project>> GetByUserIdAsync(string userId)
        {
            var user = await _userAppService.GetById(userId);

            if (user != null && user.Projects != null && user.Projects.Count() > 0)
            {
                var result = new List<Project> { };

                foreach (var projectId in user.Projects)
                {
                    var project = await _cosmosToggleDataContext.ProjectRepository.GetByIdAsync(projectId, new PartitionKey(projectId));

                    if (project != null)
                        result.Add(_mapper.Map<Project>(project));
                }

                if (result.Count == 0)
                    await _notificationContext.AddAsync(HttpStatusCode.NotFound,
                        "Project not found.", $"Project not found by user '{userId}' and projects '{string.Join(" - ", user.Projects)}'");

                return result;
            }

            return null;
        }
    }
}

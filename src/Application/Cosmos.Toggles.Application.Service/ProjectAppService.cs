using AutoMapper;
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
    public class ProjectAppService : IProjectAppService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<Project> _productValidator;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly INotificationContext _notificationContext;
        private readonly ISecurityContext _securityContext;
        private readonly IAuthAppService _authAppService;
        private readonly IUserAppService _userAppService;

        public ProjectAppService(IMapper mapper, IValidator<Project> productValidator, ICosmosToggleDataContext cosmosToggleDataContext,
            INotificationContext notificationContext, ISecurityContext securityContext, IAuthAppService authAppService, IUserAppService userAppService)
        {
            _mapper = mapper;
            _productValidator = productValidator;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
            _securityContext = securityContext;
            _authAppService = authAppService;
            _userAppService = userAppService;
        }

        public async Task CreateAsync(Project project)
        {
            _productValidator.ValidateAndThrow(project);

            var entity = _mapper.Map<Domain.Entities.Project>(project);
            await _cosmosToggleDataContext.ProjectRepository.AddAsync(entity, entity.Id);

            var user = await _securityContext.GetUserAsync();

            //TODO: using Polly
            await _userAppService.AddProjectAsync(user.Id, project.Id);
        }

        public async Task<IEnumerable<Project>> GetByUserIdAsync(string userId)
        {
            var user = await _securityContext.GetUserAsync();

            if (user != null && await _securityContext.MatchUserIdAsync(userId))
            {
                if (user.Projects != null && user.Projects.Count() > 0)
                {
                    var result = new List<Project> { };

                    foreach (var projectId in user.Projects)
                    {
                        var entity = await _cosmosToggleDataContext.ProjectRepository.GetByIdAsync(projectId, projectId);

                        if(entity != null)
                            result.Add(_mapper.Map<Project>(entity));
                    }

                    if (result.Count == 0)
                    {
                        await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Projects not found");
                        return null;
                    }

                    return result;
                }
                else
                {
                    await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Projects not found");
                }
            }

            return null;
        }

        public async Task<Project> GetAsync(string projectId)
        {
            if (!await _authAppService.UserHasAuthProjectAsync(projectId))
                return null;

            var entity = await _cosmosToggleDataContext.ProjectRepository.GetByIdAsync(projectId, projectId);

            if (entity == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, $"Project not found by id '{projectId}'");
                return null;
            }

            return _mapper.Map<Project>(entity);
        }
    }
}

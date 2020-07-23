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
    public class UserAppService : IUserAppService
    {
        private readonly IMapper _mapper;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly INotificationContext _notificationContext;
        private readonly IValidator<User> _userValidator;

        public UserAppService(IMapper mapper, ICosmosToggleDataContext cosmosToggleDataContext, INotificationContext notificationContext, IValidator<User> userValidator)
        {
            _mapper = mapper;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
            _userValidator = userValidator;
        }
        
        public async Task CreateAsync(User user)
        {
            _userValidator.ValidateAndThrow(user, ruleSet: "create");
            var currentUser = await _cosmosToggleDataContext.UserRepository.GetByEmailAsync(user.Email);

            if (currentUser == null)
            {
                var entity = _mapper.Map<Domain.Entities.User>(user);
                await _cosmosToggleDataContext.UserRepository.AddAsync(entity, new PartitionKey(entity.Id));
            }
            else
                await _notificationContext.AddAsync(HttpStatusCode.Conflict, "User already exists");
        }

        private async Task CreatePasswordAsync(string email, string password, string activationCode, string activationKey)
        {
            var user = await _cosmosToggleDataContext.UserRepository.GetByEmailAsync(email);

            if (user != null)
            {
                user.Password = password;
                await _cosmosToggleDataContext.UserRepository.UpdateAsync(user, new PartitionKey(user.Id));
            }
            else
                await _notificationContext.AddAsync(HttpStatusCode.Conflict, "User already exists");
        }
    }
}

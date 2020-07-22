using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service
{
    public class UserAppService : IUserAppService
    {
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly IValidator<User> _userValidator;

        public UserAppService(ICosmosToggleDataContext cosmosToggleDataContext, IValidator<User> userValidator)
        {
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _userValidator = userValidator;
        }
        public Task CreateAsync(User dto)
        {
            _userValidator.ValidateAndThrow(dto, ruleSet: "create");
            var user = _cosmosToggleDataContext.UserRepository.GetByEmailAsync(dto.Email);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}

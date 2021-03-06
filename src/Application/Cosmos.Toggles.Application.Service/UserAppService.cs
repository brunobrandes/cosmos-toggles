﻿using AutoMapper;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Enum;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
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

        public UserAppService(IMapper mapper, ICosmosToggleDataContext cosmosToggleDataContext, INotificationContext notificationContext,
            IValidator<User> userValidator, IAuthAppService authAppService)
        {
            _mapper = mapper;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
            _userValidator = userValidator;
        }

        private async Task CreatePasswordAsync(string email, string password, string activationCode, string activationKey)
        {
            var user = await _cosmosToggleDataContext.UserRepository.GetByEmailAsync(email);

            if (user != null)
            {
                user.Password = password;
                user.Status = UserStatus.Activated;
                await _cosmosToggleDataContext.UserRepository.UpdateAsync(user, user.Id);
            }
            else
            {
                await _notificationContext.AddAsync(HttpStatusCode.Conflict, "User already exists");
            }

        }

        public async Task AddProjectAsync(string userId, string projectId)
        {
            var user = await _cosmosToggleDataContext.UserRepository.GetByIdAsync(userId, userId);

            if (user != null)
            {
                if (user.Projects == null)
                {
                    user.Projects = new List<string> { };
                }

                if (!user.Projects.Contains(projectId))
                {
                    user.Projects = user.Projects.Append(projectId).ToList();
                    await _cosmosToggleDataContext.UserRepository.UpdateAsync(user, user.Id);
                }
            }
        }

        public async Task CreateAsync(User user)
        {
            _userValidator.ValidateAndThrow(user);
            var currentUser = await _cosmosToggleDataContext.UserRepository.GetByEmailAsync(user.Email);

            if (currentUser == null)
            {
                var entity = _mapper.Map<Domain.Entities.User>(user);
                await _cosmosToggleDataContext.UserRepository.AddAsync(entity, entity.Id);
            }
            else
                await _notificationContext.AddAsync(HttpStatusCode.Conflict, "User already exists");
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            var entity = await _cosmosToggleDataContext.UserRepository.GetByIdAsync(userId, userId);

            if (entity == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.NotFound, "User not found");
                return null;
            }

            return _mapper.Map<User>(entity);
        }

    }
}

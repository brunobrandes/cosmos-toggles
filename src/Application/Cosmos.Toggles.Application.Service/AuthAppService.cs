using AutoMapper;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Enum;
using Cosmos.Toggles.Domain.Service.Interfaces;
using FluentValidation;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service
{
    public class AuthAppService : IAuthAppService
    {
        const int EXPIRES = 1500;

        private readonly IMapper _mapper;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly INotificationContext _notificationContext;
        private readonly IValidator<Login> _loginValidator;
        private readonly ITokenService _tokenService;
        private readonly ISecurityContext _securityContext;

        public AuthAppService(IMapper mapper, ICosmosToggleDataContext cosmosToggleDataContext,
            INotificationContext notificationContext, IValidator<Login> loginValidator,
            ITokenService tokenService, ISecurityContext securityContext)
        {
            _mapper = mapper;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _notificationContext = notificationContext;
            _loginValidator = loginValidator;
            _tokenService = tokenService;
            _securityContext = securityContext;
        }

        public async Task<bool> UserHasAuthProjectAsync(string projectId)
        {
            var user = await _securityContext.GetUserAsync();

            var friendlyMessage = $"User unauthorized in project '{projectId}'.";

            if (user == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.Unauthorized, $"User not found in claims", friendlyMessage);
                return false;
            }

            if (user.Projects == null || !user.Projects.Contains(projectId))
            {
                await _notificationContext.AddAsync(HttpStatusCode.Unauthorized, $"User projects list not contains '{projectId}'", friendlyMessage);
                return false;
            }

            return true;
        }

        public async Task<Token> LoginAsync(Login login, string ipAddress)
        {
            _loginValidator.ValidateAndThrow(login);

            var userEntity = await _cosmosToggleDataContext.UserRepository.GetByEmailPasswordAsync(login.Email, login.Password);

            if (userEntity == null || userEntity.Status != UserStatus.Activated)
            {
                await _notificationContext.AddAsync(HttpStatusCode.Unauthorized, "Unauthorized", "Invalid auth data or status.");
                return null;
            }

            var dateTimeNow = DateTime.UtcNow;

            var result = new RefreshToken
            {
                UserId = userEntity.Id,
                Key = await _tokenService.CreateKeyAsync(),
                Jwt = await _tokenService.CreateJwtAsync(userEntity.Id, userEntity.Name, userEntity.Email, EXPIRES),
                Created = dateTimeNow,
                CreatedIp = ipAddress,
                Expires = dateTimeNow.AddSeconds(EXPIRES)
            };

            var refreshToken = _mapper.Map<Domain.Entities.RefreshToken>(result, opts =>
            {
                opts.Items["UserId"] = userEntity.Id;
            });

            await _cosmosToggleDataContext.RefreshTokenRepository.AddAsync(refreshToken, refreshToken.UserId);

            return _mapper.Map<Token>(result);
        }

        public async Task<Token> RefreshAsync(string key, string userId, string ipAddress)
        {
            var refreshTokenEntity = await _cosmosToggleDataContext.RefreshTokenRepository.GetByKeyUserIdAsync(key, userId);

            if (refreshTokenEntity == null)
            {
                await _notificationContext.AddAsync(HttpStatusCode.Unauthorized, "The access token expired", null);
                return null;
            }

            refreshTokenEntity.Revoked = DateTime.UtcNow;
            refreshTokenEntity.RevokedIp = ipAddress;
            refreshTokenEntity.Ttl = 1;

            _ = _cosmosToggleDataContext.RefreshTokenRepository.TryUpdateAsync(refreshTokenEntity, userId);

            var user = await _tokenService.ExtractUserAsync(refreshTokenEntity.Jwt);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Key = await _tokenService.CreateKeyAsync(),
                Jwt = await _tokenService.CreateJwtAsync(user.Id, user.Name, user.Email, EXPIRES),
                Created = DateTime.UtcNow,
                CreatedIp = ipAddress,
                Expires = DateTime.UtcNow.AddSeconds(EXPIRES)
            };

            await _cosmosToggleDataContext.RefreshTokenRepository.AddAsync(_mapper.Map<Domain.Entities.RefreshToken>(refreshToken), user.Id);
            return _mapper.Map<Token>(refreshToken);
        }
    }
}

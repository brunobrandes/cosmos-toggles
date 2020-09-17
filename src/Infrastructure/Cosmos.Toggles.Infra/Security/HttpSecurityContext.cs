using AutoMapper;
using Azure.Cosmos;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Infra.Security
{
    public class HttpSecurityContext : ISecurityContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly IMapper _mapper;
        private readonly INotificationContext _notificationContext;

        public HttpSecurityContext(IHttpContextAccessor httpContextAccessor, ICosmosToggleDataContext cosmosToggleDataContext,
            IMapper mapper, INotificationContext notificationContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _mapper = mapper;
            _notificationContext = notificationContext;
        }

        public async Task<User> GetUserAsync()
        {
            var userId = await this.GetUserIdAsync();

            if (!string.IsNullOrEmpty(userId))
            {
                var entity = await _cosmosToggleDataContext.UserRepository.GetByIdAsync(userId, new PartitionKey(userId));

                if (entity != null)
                    return _mapper.Map<User>(entity);
            }

            return null;
        }

        public async Task<string> GetUserIdAsync()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null &&
                _httpContextAccessor.HttpContext.User.Identity != null)
            {
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null && identity.Claims != null)
                {
                    var userId = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (string.IsNullOrEmpty(userId))
                        await _notificationContext.AddAsync(HttpStatusCode.Unauthorized, "Unauthorized", "Claim 'UserId' not found.");

                    return userId;

                }
            }

            return string.Empty;
        }

        public async Task<bool> MatchUserIdAsync(string userId)
        {
            if (await this.GetUserIdAsync() == userId)
                return true;

            await _notificationContext.AddAsync(HttpStatusCode.Unauthorized, "Unauthorized", "Claim 'UserId' not match.");
            return false;
        }
    }
}

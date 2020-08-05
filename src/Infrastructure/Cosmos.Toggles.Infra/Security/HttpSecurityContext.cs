using AutoMapper;
using Azure.Cosmos;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Infra.Security
{
    public class HttpSecurityContext : ISecurityContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;
        private readonly IMapper _mapper;

        public HttpSecurityContext(IHttpContextAccessor httpContextAccessor, ICosmosToggleDataContext cosmosToggleDataContext, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _mapper = mapper;
        }

        public async Task<User> GetUserAsync()
        {
            User user = null;

            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null &&
                _httpContextAccessor.HttpContext.User.Identity != null)
            {
                var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

                if (identity != null && identity.Claims != null)
                {
                    var userId = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var entity = await _cosmosToggleDataContext.UserRepository.GetByIdAsync(userId, new PartitionKey(userId));

                        if (entity != null)
                            user = _mapper.Map<User>(entity);
                    }
                }
            }

            return user;
        }
    }
}

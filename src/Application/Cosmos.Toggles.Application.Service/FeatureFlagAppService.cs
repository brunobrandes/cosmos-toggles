using AutoMapper;
using Cosmos.Toggles.Application.Service.Interfaces;
using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Entities.Interfaces;
using Cosmos.Toggles.Domain.Service.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Application.Service
{
    public class FeatureFlagAppService : IFeatureFlagAppService
    {
        private readonly IMapper _mapper;
        private readonly ICosmosToggleDataContext _cosmosToggleDataContext;


        public FeatureFlagAppService(IMapper mapper, ICosmosToggleDataContext cosmosToggleDataContext)
        {
            _cosmosToggleDataContext = cosmosToggleDataContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Get feature flag by environment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="environmentId"></param>
        /// <returns></returns>
        public async Task<FeatureFlag> GetByEnvironmentAsync(string projectId, string environmentId, string flagId)
        {
            try
            {
                var entity = await _cosmosToggleDataContext.FlagRepository.GetByEnvironmentAsync(projectId, environmentId, flagId);
                return _mapper.Map<FeatureFlag>(entity, opts =>
                {

                    opts.Items["Code"] = ((int)HttpStatusCode.OK).ToString();
                    opts.Items["Description"] = $"Feature flag query successfully";
                });
            }
            catch (Exception ex)
            {
                return ex.ToFeatureFlag(flagId);
            }
        }
    }
}

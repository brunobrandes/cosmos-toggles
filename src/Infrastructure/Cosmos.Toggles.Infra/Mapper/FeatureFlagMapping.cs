using AutoMapper;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Domain.Enum;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class FeatureFlagMapping : Profile
    {
        public FeatureFlagMapping()
        {
            CreateMap<Flag, Domain.DataTransferObject.FeatureFlag>()
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Enabled ? FeatureFlagStatus.Enabled : FeatureFlagStatus.Disabled))
               .ForMember(dest => dest.Code, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["Code"]))
               .ForMember(dest => dest.Description, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["Description"]));
        }
    }
}

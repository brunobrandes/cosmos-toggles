using AutoMapper;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Infra.Mapper.Custom;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class FlagMapping : Profile
    {
        public FlagMapping()
        {
            CreateMap<Flag, Domain.DataTransferObject.Flag>()
                .ForMember(dest => dest.Expiration, opt => opt.MapFrom<ExpirationResolver>())
                .ReverseMap()
                .ForMember(dest => dest.Ttl, opt => opt.ConvertUsing(new TimeToLiveConverter(), src => src.Expiration));
        }
    }
}

using AutoMapper;
using Cosmos.Toggles.Domain.Entities;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class FlagMapping : Profile
    {
        public FlagMapping()
        {
            CreateMap<Flag, Domain.DataTransferObject.Flag>()
                .ReverseMap()
                .ForMember(dest => dest.Ttl, opt => opt.MapFrom(src => src.Ttl > 0 ? src.Ttl : -1));
        }
    }
}

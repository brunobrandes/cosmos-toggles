using AutoMapper;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Infra.Mapper.Custom;
using System;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class FlagMapping : Profile
    {
        public FlagMapping()
        {
            CreateMap<Flag, Domain.DataTransferObject.Flag>()
                .ForMember(dest => dest.Expires, opt => opt.MapFrom<FlagExpiresResolver>())
                .ReverseMap()
                   .ForMember(x => x.Created, opt => opt.MapFrom(o => DateTime.UtcNow))
                   .ForMember(dest => dest.Ttl, opt => opt.ConvertUsing(new TimeToLiveConverter(), src => src.Expires));
        }
    }
}

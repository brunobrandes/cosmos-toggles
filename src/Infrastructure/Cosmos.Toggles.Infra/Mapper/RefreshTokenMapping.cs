using AutoMapper;
using Cosmos.Toggles.Domain.Entities;
using Cosmos.Toggles.Infra.Mapper.Custom;
using System;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class RefreshTokenMapping : Profile
    {
        public RefreshTokenMapping()
        {
            CreateMap<RefreshToken, Domain.DataTransferObject.RefreshToken>()
                .ForMember(dest => dest.Expires, opt => opt.MapFrom<RefreshTokenExpiresResolver>())
                .ReverseMap()
                    .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created == DateTime.MinValue ? DateTime.UtcNow : src.Created))
                    .ForMember(dest => dest.Ttl, opt => opt.ConvertUsing(new TimeToLiveConverter(), src => src.Expires));
        }
    }
}

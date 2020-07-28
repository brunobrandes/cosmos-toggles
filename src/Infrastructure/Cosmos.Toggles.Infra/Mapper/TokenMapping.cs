using AutoMapper;
using Cosmos.Toggles.Domain.DataTransferObject;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class TokenMapping : Profile
    {
        public TokenMapping()
        {
            CreateMap<Token, RefreshToken>()
                .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedIp, opt => opt.Ignore())
                .ForMember(dest => dest.Revoked, opt => opt.Ignore())
                .ForMember(dest => dest.RevokedIp, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}

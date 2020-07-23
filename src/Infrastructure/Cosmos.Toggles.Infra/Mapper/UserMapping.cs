using AutoMapper;
using Cosmos.Toggles.Domain.Entities;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, Domain.DataTransferObject.User>()
                .ReverseMap()
                  .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}

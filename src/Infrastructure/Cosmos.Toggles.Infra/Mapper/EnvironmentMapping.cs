using AutoMapper;
using Cosmos.Toggles.Domain.Entities;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class EnvironmentMapping : Profile
    {
        public EnvironmentMapping()
        {
            CreateMap<Environment, Domain.DataTransferObject.Environment>()
                .ReverseMap();
        }
    }
}

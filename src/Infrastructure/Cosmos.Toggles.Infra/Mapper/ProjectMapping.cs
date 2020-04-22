using AutoMapper;
using Cosmos.Toggles.Domain.Entities;

namespace Cosmos.Toggles.Infra.Mapper
{
    public class ProjectMapping : Profile
    {
        public ProjectMapping()
        {
            CreateMap<Project, Domain.DataTransferObject.Project>()
                .ReverseMap();
        }
    }
}

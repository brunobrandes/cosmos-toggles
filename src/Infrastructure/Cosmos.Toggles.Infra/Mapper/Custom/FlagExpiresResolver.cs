using AutoMapper;
using Cosmos.Toggles.Domain.Entities;
using System;

namespace Cosmos.Toggles.Infra.Mapper.Custom
{
    public class FlagExpiresResolver : IValueResolver<Flag, Domain.DataTransferObject.Flag, DateTime?>
    {
        public DateTime? Resolve(Flag source, Domain.DataTransferObject.Flag destination, DateTime? destMember, ResolutionContext context)
        {
            return ExpiresResolver<Flag, Domain.DataTransferObject.Flag>.Resolve(source, destination);
        }
    }
}

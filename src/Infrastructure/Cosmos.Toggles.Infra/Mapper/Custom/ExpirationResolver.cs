using AutoMapper;
using Cosmos.Toggles.Domain.Entities;
using System;

namespace Cosmos.Toggles.Infra.Mapper.Custom
{
    public class ExpirationResolver : IValueResolver<Flag, Domain.DataTransferObject.Flag, DateTime?>
    {
        public DateTime? Resolve(Flag source, Domain.DataTransferObject.Flag destination, DateTime? destMember, ResolutionContext context)
        {
            if (source.Ttl > 0)
            {
                return source.Created.AddSeconds(source.Ttl);
            }

            return null;
        }
    }
}

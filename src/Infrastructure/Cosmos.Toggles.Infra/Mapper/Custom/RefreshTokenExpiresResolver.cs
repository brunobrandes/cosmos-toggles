using AutoMapper;
using Cosmos.Toggles.Domain.Entities;
using System;

namespace Cosmos.Toggles.Infra.Mapper.Custom
{
    public class RefreshTokenExpiresResolver : IValueResolver<RefreshToken, Domain.DataTransferObject.RefreshToken, DateTime>
    {
        public DateTime Resolve(RefreshToken source, Domain.DataTransferObject.RefreshToken destination, DateTime destMember, ResolutionContext context)
        {
            return ExpiresResolver<RefreshToken, Domain.DataTransferObject.RefreshToken>.Resolve(source, destination).Value;
        }
    }
}

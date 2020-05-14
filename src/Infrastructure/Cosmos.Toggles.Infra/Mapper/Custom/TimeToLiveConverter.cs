using AutoMapper;
using System;

namespace Cosmos.Toggles.Infra.Mapper.Custom
{
    public class TimeToLiveConverter : IValueConverter<DateTime?, int>
    {
        public int Convert(DateTime? sourceMember, ResolutionContext context)
        {
            if (sourceMember.HasValue && sourceMember != DateTime.MinValue)
            {
                var timeSpan = sourceMember.Value.Subtract(DateTime.UtcNow);
                return (int)Math.Ceiling(timeSpan.TotalSeconds);
            }
            return -1;
        }
    }
}

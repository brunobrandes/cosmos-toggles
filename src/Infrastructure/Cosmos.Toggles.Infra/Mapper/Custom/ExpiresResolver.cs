using Cosmos.Db.Sql.Api.Domain.Entities;
using System;

namespace Cosmos.Toggles.Infra.Mapper.Custom
{
    public static class ExpiresResolver<TSource, TDestination>
         where TSource : Entity
         where TDestination : class
    {
        public static DateTime? Resolve(TSource source, TDestination destination)
        {
            var created = source.GetType().GetProperty("Created").GetValue(source, null);

            if (source.Ttl > 0 && created != null)
            {
                return ((DateTime)created).AddSeconds(source.Ttl);
            }

            return null;
        }
    }
}

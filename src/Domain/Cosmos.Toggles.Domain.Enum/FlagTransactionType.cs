using System;

namespace Cosmos.Toggles.Domain.Enum
{
    [Flags]
    public enum FlagTransactionType
    {
        None = 0,
        Create = 1,
        Read = 2,
        Update = 4,
        Delete = 8
    }
}

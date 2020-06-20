using Cosmos.Toggles.Domain.Enum;

namespace Cosmos.Toggles.Domain.Entities
{
    public class FlagUserProfile
    {
        public string UserId { get; set; }
        public FlagTransactionType FlagTransactionType { get; set; }
    }
}

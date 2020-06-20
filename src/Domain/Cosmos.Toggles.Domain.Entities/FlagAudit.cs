using Cosmos.Toggles.Domain.Enum;

namespace Cosmos.Toggles.Domain.Entities
{
    public class FlagAudit
    {
        public Flag Flag { get; set; }
        public User User { get; set; }
        public FlagTransactionType FlagTransactionType { get; set; }
    }
}

using Cosmos.Toggles.Domain.Enum;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class FlagAudit
    {
        /// <summary>
        /// Flag
        /// </summary>
        public Flag Flag { get; set; }
        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Flag transaction type user has authorization
        /// </summary>
        public FlagTransactionType FlagTransactionType { get; set; }
    }
}

using Cosmos.Toggles.Domain.Enum;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class FlagUserProfile
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Flag transaction type user has authorization
        /// </summary>
        public FlagTransactionType FlagTransactionType { get; set; }
    }
}

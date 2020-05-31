using System.Reflection.Emit;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class User
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}

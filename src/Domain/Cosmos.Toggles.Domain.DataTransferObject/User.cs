using System.Collections.Generic;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class User
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Projects that managed
        /// </summary>
        public IEnumerable<string> Projects { get; set; }
    }
}

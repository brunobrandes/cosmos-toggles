namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class Auth
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Secret
        /// </summary>
        public string Secret { get; set; }
    }
}

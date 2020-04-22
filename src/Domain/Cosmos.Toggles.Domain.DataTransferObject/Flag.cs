namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class Flag
    {
        /// <summary>
        /// Environment
        /// </summary>
        public Environment Environment { get; set; }
        /// <summary>
        /// identifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Enabled
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// Time to live
        /// </summary>
        public int Ttl { get; set; }
    }
}

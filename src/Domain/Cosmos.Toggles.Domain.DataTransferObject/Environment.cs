namespace Cosmos.Toggles.Domain.DataTransferObject
{
    /// <summary>
    /// Enviroment
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// Project parent
        /// </summary>
        public Project Project { get; set; }
        /// <summary>
        /// Identifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }
}

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class ProjectUserProfile
    {
        /// <summary>
        /// Project identifier
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// Flag user profile
        /// </summary>
        public FlagUserProfile FlagUserProfile { get; set; }

    }
}

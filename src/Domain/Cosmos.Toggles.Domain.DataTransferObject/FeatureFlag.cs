using Cosmos.Toggles.Domain.Enum;

namespace Cosmos.Toggles.Domain.DataTransferObject
{
    public class FeatureFlag
    {
        /// <summary>
        /// Flag identifier
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Feature flag status
        /// </summary>
        public FeatureFlagStatus Status { get; set; }
        /// <summary>
        /// Code of get status result
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Description of get status result
        /// </summary>
        public string Description { get; set; }
    }
}

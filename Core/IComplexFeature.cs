namespace MultiTenancy.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a feature with subfeatures
    /// </summary>
    public interface IComplexFeature : IFeature
    {
        /// <summary>
        /// Gets the subfeatures of this feature
        /// </summary>
        IEnumerable<IFeature> SubFeatures { get; }
    }
}
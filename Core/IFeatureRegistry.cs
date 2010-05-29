namespace MultiTenancy.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Container for features by a tenant of the application
    /// </summary>
    public interface IFeatureRegistry
    {
        /// <summary>
        /// Gets the features used by this tenant
        /// </summary>
        IEnumerable<IFeature> Features { get; }

        /// <summary>
        /// Gets whether a feature is enabled for a given feature path
        /// </summary>
        /// <param name="featurePath">Feature path with values after the first string being subfeatures</param>
        /// <returns>Value indicating whether a feature is enabled</returns>
        bool IsEnabled(IEnumerable<string> featurePath);
    }
}
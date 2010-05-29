namespace MultiTenancy.Core
{
    /// <summary>
    /// Specifies a feature used by the application
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// Gets the name of the feature used by the application
        /// </summary>
        string FeatureName { get; }
    }
}
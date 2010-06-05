namespace MultiTenancy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper utilities for <see cref="MultiTenancy.Core.IFeature"/>
    /// </summary>
    public static class Features
    {
        /// <summary>
        /// Gets whether a feature path is valid for the features in the registry
        /// </summary>
        /// <param name="registry">Feature registry where the Features property is used to determine if the path is valid</param>
        /// <param name="featurePath">Feature path to the highest-level feature</param>
        /// <returns>Value indicating whether the feature registry contains a valid feature for the given path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="registry"/> or <paramref name="featurePath"/> is null</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when <paramref name="featurePath"/> is empty</exception>
        public static bool IsEnabled(IFeatureRegistry registry, IEnumerable<string> featurePath)
        {
            Ensure.Argument.NotNull(registry, "registry");
            return IsEnabled(registry.Features, featurePath);
        }

        /// <summary>
        /// Gets whether a feature path is valid for the features in the feature set
        /// </summary>
        /// <param name="features">Top-level features</param>
        /// <param name="featurePath">Feature path to the highest-level feature</param>
        /// <returns>Value indicating whether the feature path is valid for a feature in <paramref name="features"/></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="features"/> or <paramref name="featurePath"/> is null</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when <paramref name="featurePath"/> is empty</exception>
        public static bool IsEnabled(IEnumerable<IFeature> features, IEnumerable<string> featurePath)
        {
            Ensure.Argument.NotNull(features, "features");
            Ensure.Argument.NotNull(featurePath, "featurePath");
            Ensure.That<InvalidOperationException>(featurePath.Any(), "Feature Path must contain at least one top-level feature");

            // feature names are case insensitive
            IFeature current = FindFeature(features, featurePath.First());

            // skip the first value
            featurePath = featurePath.Skip(1);

            // loop through the entire path
            while (featurePath.Any())
            {
                // path was not found
                if (current == null)
                    return false;

                // see if the feature has subfeatures (Complex)
                var asComplex = current as IComplexFeature;
                if (asComplex == null) // feature doesn't have subfeatures, it passes
                    return true;

                current = FindFeature(asComplex.SubFeatures, featurePath.First());

                featurePath = featurePath.Skip(1);
            }

            return current != null;
        }

        /// <summary>
        /// Finds a feature in <paramref name="features"/> where the feature name is <paramref name="pathValue"/>
        /// </summary>
        /// <param name="features">Features from which to search</param>
        /// <param name="pathValue">Feature/Subfeature name</param>
        /// <returns>Found feature in the enumerable. Null if not found.</returns>
        private static IFeature FindFeature(IEnumerable<IFeature> features, string pathValue)
        {
            return features.SingleOrDefault(f => f.FeatureName.Trim().Equals(pathValue.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using MultiTenancy.Core;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace MultiTenancy.Tests.Core.Specifications
{
    public class FeatureTests
    {
        [Theory]
        [InlineData("a", "A")]
        [InlineData("A", "a")]
        [InlineData("a", " a")]
        [InlineData(" a", "a")]
        [InlineData("a", "a ")]
        [InlineData("a ", "a")]
        public void Features_IsEnabled_ReturnsTrue_WhenFeatureRegistryContainsFeature(string specFeature, string feature)
        {
            var featureValue = new Feature(feature);
            Assert.True(Features.IsEnabled(SetupRegistry(featureValue), new[] { specFeature }));
        }

        [Fact]
        public void Features_IsEnabled_ReturnsFalse_WhenFeatureRegistryDoesntContainFeature()
        {
            Assert.False(Features.IsEnabled(SetupRegistry(), new[] { "a" }));
        }

        [Fact]
        public void Features_IsEnabled_ReturnsFalse_WhenComplexFeatureDoesntContainSubfeature()
        {
            // test feature a/b where b is not present in a
            var feature = new ComplexFeature("a", new Feature("c"));
            Assert.False(Features.IsEnabled(SetupRegistry(feature), new[] { "a", "b" }));
        }

        [Fact]
        public void Features_IsEnabled_ReturnsTrue_WhenComplexFeatureContainsSubfeature()
        {
            // test feature a/b/c
            var feature = new ComplexFeature("a", new ComplexFeature("b", new Feature("c")));
            Assert.True(Features.IsEnabled(SetupRegistry(feature), new[] { "a", "b", "c" }));
        }

        [Fact]
        public void Features_IsEnabled_ReturnsTrue_WhenPathIsMoreComplexThanFeatureInRegistry()
        {
            // test feature a/b/c where value in registry is just a (signifying all features registered)
            var feature = new Feature("a");
            var attribute = new FeatureAttribute("a", "b", "c");
            Assert.True(Features.IsEnabled(SetupRegistry(feature), new[] { "a", "b", "c" }));
        }

        // ---- HELPERS ----

        private IFeatureRegistry SetupRegistry(params IFeature[] features)
        {
            var registry = new Mock<IFeatureRegistry>();
            registry.SetupGet(x => x.Features).Returns(features ?? Enumerable.Empty<IFeature>());
            registry.Setup(r => r.IsEnabled(It.IsAny<IEnumerable<string>>())).Returns(false);
            return registry.Object;
        }

        private class ComplexFeature : IComplexFeature
        {
            public ComplexFeature(string name, params IFeature[] features)
            {
                SubFeatures = features;
                FeatureName = name;
            }

            public IEnumerable<IFeature> SubFeatures { get; private set; }
            public string FeatureName { get; private set; }
        }

        private class Feature : IFeature
        {
            public Feature(string name)
            {
                FeatureName = name;
            }

            public string FeatureName { get; private set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using MultiTenancy.Core;
using StructureMap;
using Xunit;

namespace MultiTenancy.Tests.Core.Specifications
{
    public class FeatureAttributeTests
    {
        private ActionExecutingContext blankContext = new Mock<ActionExecutingContext>().Object;

        [Fact]
        public void FeatureAttribute_Ctr_ThrowsException_GivenInvalidFeatureName()
        {
            Assert.Throws<ArgumentNullException>(() => new FeatureAttribute(featurePath: null));
        }

        [Fact]
        public void FeatureAttribute_Ctr_SetsFeaturePathProperty()
        {
            const string feature = "#@#$%^*";
            var attribute = new FeatureAttribute(feature, feature);
            Assert.Equal(feature + feature, attribute.FeaturePath.ConcatAll());
        }

        [Fact]
        public void FeatureAttribute_Vaildate_ThrowsException_GivenNullContainer()
        {
            Assert.Throws<ArgumentNullException>(() => new FeatureAttribute("a").Validate(null, blankContext));
        }

        [Fact]
        public void FeatureAttribute_Vaildate_ReturnsTrue_WhenContainerDoesntContainFeatureRegistry()
        {
            var container = new Container();
            Assert.True(new FeatureAttribute("a").Validate(container, blankContext));
        }

        [Fact]
        public void FeatureAttribute_Vaildate_ReturnsTrue_WhenFeatureRegistryDeterminesValueIsEnabled()
        {
            Assert.True(new FeatureAttribute("a").Validate(SetupContainer(true), blankContext));
        }

        [Fact]
        public void FeatureAttribute_Vaildate_ReturnsFalse_WhenFeatureRegistryDeterminesValueIsNotEnabled()
        {
            Assert.False(new FeatureAttribute("a").Validate(SetupContainer(false), blankContext));
        }

        [Fact]
        public void FeatureAttribute_Validate_CallsIsEnabledWithControllerAction_WithPathEqualTo()
        {
            const string action = "action", controller = "FeatureController";
            IEnumerable<string> path = Enumerable.Empty<string>();
            var registry = new Mock<IFeatureRegistry>();
            registry.SetupGet(x => x.Features).Returns(Enumerable.Empty<IFeature>());
            registry.Setup(r => r.IsEnabled(It.IsAny<IEnumerable<string>>())).Returns<IEnumerable<string>>(p => { path = p; return true; });
            var container = new Container();
            container.Inject<IFeatureRegistry>(registry.Object);
            var context = new Mock<ActionExecutingContext>();
            context.Setup(x => x.ActionDescriptor.ActionName).Returns(action);
            context.Setup(x => x.ActionDescriptor.ControllerDescriptor.ControllerName).Returns(controller);

            new FeatureAttribute().Validate(container, context.Object);

            Assert.Equal(new[] { controller.Without("Controller"), action }.ConcatAll(), path.ConcatAll());
        }

        // ---- HELPERS ----

        private Container SetupContainer(bool enabled)
        {
            var registry = new Mock<IFeatureRegistry>();
            registry.SetupGet(x => x.Features).Returns(Enumerable.Empty<IFeature>());
            registry.Setup(r => r.IsEnabled(It.IsAny<IEnumerable<string>>())).Returns(enabled);
            var container = new Container();
            container.Inject<IFeatureRegistry>(registry.Object);
            return container;
        }
    }
}

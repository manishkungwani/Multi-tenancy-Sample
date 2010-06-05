using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using MultiTenancy.Core;
using StructureMap;
using Xunit;

namespace MultiTenancy.Tests.Core
{
    public class ContainerControllerFactoryTests
    {
        [Fact]
        public void ContainerControllerFactory_Ctr_ThrowsException_GivenNullResolver()
        {
            Assert.Throws<ArgumentNullException>(() => new ContainerControllerFactory(null));
        }

        [Fact]
        public void ContainerControllerFactory_CreateController_ReturnsNull_WhenGetTypeReturnsNull()
        {
            var factory = new TestContainerControllerFactory(new Mock<IContainerResolver>().Object);
            factory.GetControllerTypeDelegate = () => null;
            Assert.Null(factory.CreateController(null, ""));
        }

        [Fact]
        public void ContainerControllerFactory_CreateController_ReturnsExpectedTypeFromContainer_GivenContainerAndProperType()
        {
            var container = new Container();
            var controller = new ProperController();
            container.Inject<ProperController>(controller);
            var factory = new TestContainerControllerFactory(new ContainerResolver(container));
            factory.GetControllerTypeDelegate = () => typeof(ProperController);

            Assert.Same(controller, factory.CreateController(null, ""));
        }

        [Fact]
        public void ContainerControllerFactory_CreateController_ReturnsControllerWithActionInvoker_WhenActionInvokerIsNotContainerActionInvoker()
        {
            var container = new Container();
            var controller = new ProperController();
            container.Inject<ProperController>(controller);
            var factory = new TestContainerControllerFactory(new ContainerResolver(container));
            factory.GetControllerTypeDelegate = () => typeof(ProperController);

            Assert.IsType<ContainerControllerActionInvoker>((factory.CreateController(null, "") as ProperController).ActionInvoker);
        }

        [Fact]
        public void ContainerControllerFactory_CreateController_DoesntSetActionInvoker_WhenOfCorrectType()
        {
            var container = new Container();
            var containerResolver = new ContainerResolver(container);
            var controller = new ProperController();
            var actionInvoker = new ContainerControllerActionInvoker(containerResolver);

            controller.ActionInvoker = actionInvoker;
            container.Inject<ProperController>(controller);
            var factory = new TestContainerControllerFactory(containerResolver);
            factory.GetControllerTypeDelegate = () => typeof(ProperController);

            Assert.Same(actionInvoker, (factory.CreateController(null, "") as ProperController).ActionInvoker);
        }

        [Fact]
        public void ContainercontrollerFactory_GetTypesFor_ReturnsEmpty_WhenNoControllersInContainer()
        {
            var container = new Container();
            var types = ContainerControllerFactory.GetControllersFor(container);
            Assert.Empty(types);
        }

        [Fact]
        public void ContainerControllerFactory_GetTypesFor_ReturnsAllControllerTypesInContainer()
        {
            Func<IEnumerable<Type>, string> typeToString = _types => _types.OrderBy(x => x.Name).Select(x => x.Name).ConcatAll(",");

            var container = new Container();
            var mockController = new Mock<IController>().Object;
            container.Inject<IController>(new ProperController());
            container.Inject<IController>(mockController);

            var types = ContainerControllerFactory.GetControllersFor(container);

            Assert.Equal(typeToString(new[] { typeof(ProperController), mockController.GetType() }),
                         typeToString(types));
        }

        [Fact]
        public void ContainerControllerFactory_GetControllerType_ReturnsNull_WhenContainerDoesntContainController()
        {
            var container = new Container();
            var factory = new TestContainerControllerFactory(new ContainerResolver(container));
            Assert.Null(factory.GetControllerType_(new Mock<RequestContext>().Object, "Proper"));
        }

        [Fact]
        public void ContainerControllerFactory_GetControllerType_ReturnsType_WhenContainerContainsControllerWithType()
        {
            var container = new Container();
            container.Inject<IController>(new ProperController());
            var factory = new TestContainerControllerFactory(new ContainerResolver(container));
            Assert.Equal(typeof(ProperController), factory.GetControllerType_(new Mock<RequestContext>().Object, "Proper"));
        }

        [Fact]
        public void ContainerControllerFactory_GetControllerType_ReturnsType_WhenContainerContainsController_SpecifiedControllerContainsControllerAppendage()
        {
            var container = new Container();
            container.Inject<IController>(new ProperController());
            var factory = new TestContainerControllerFactory(new ContainerResolver(container));
            Assert.Equal(typeof(ProperController), factory.GetControllerType_(new Mock<RequestContext>().Object, "ProperController"));
        }

        [Fact]
        public void ContainerControllerFactory_GetControllerType_ReturnsType_WhenContainerContainsController_SpecifiedControllerIsInRandomCase()
        {
            var container = new Container();
            container.Inject<IController>(new ProperController());
            var factory = new TestContainerControllerFactory(new ContainerResolver(container));
            Assert.Equal(typeof(ProperController), factory.GetControllerType_(new Mock<RequestContext>().Object, "PrOpEr"));
        }

        [Fact]
        public void ContainerControllerFactory_ReleaseController_DoesntThrowException_GivenNullController()
        {
            Assert.DoesNotThrow(() => new ContainerControllerFactory(new ContainerResolver(new Container())).ReleaseController(null));
        }

        [Fact]
        public void ContainerControllerFactory_ReleaseController_DoesntThrowException_GivenNonDisposableController()
        {
            Assert.DoesNotThrow(() => new ContainerControllerFactory(new ContainerResolver(new Container())).ReleaseController(new ProperController()));
        }

        [Fact]
        public void ContainerControllerFactory_ReleaseController_CallsDispose_GivenDisposableController()
        {
            var controller = new DisposableController();
            new ContainerControllerFactory(new ContainerResolver(new Container())).ReleaseController(controller);
            Assert.True(controller.Disposed);
        }

        private class DisposableController : IController, IDisposable
        {
            public DisposableController()
            {
                Disposed = false;
            }

            public bool Disposed { get; set; }

            public void Dispose()
            {
                Disposed = true;
            }

            public void Execute(RequestContext requestContext)
            {
                throw new NotImplementedException();
            }
        }

        private class ContainerResolver : IContainerResolver
        {
            public ContainerResolver(IContainer container)
            {
                this.container = container;
            }

            private IContainer container;

            public IContainer Resolve(RequestContext context)
            {
                return container;
            }
        }

        public class TestContainerControllerFactory : ContainerControllerFactory
        {
            public TestContainerControllerFactory(IContainerResolver resolver)
                : base(resolver)
            {

            }

            protected override Type GetControllerType(System.Web.Routing.RequestContext requestContext, string controllerName)
            {
                return GetControllerType_(requestContext, controllerName);
            }

            public Func<Type> GetControllerTypeDelegate { get; set; }

            public virtual Type GetControllerType_(System.Web.Routing.RequestContext requestContext, string controllerName)
            {
                if (GetControllerTypeDelegate != null)
                    return GetControllerTypeDelegate();
                return base.GetControllerType(requestContext, controllerName);
            }

            public override System.Web.Mvc.IController CreateController(RequestContext requestContext, string controllerName)
            {
                return base.CreateController(requestContext, controllerName);
            }
        }
    }
}

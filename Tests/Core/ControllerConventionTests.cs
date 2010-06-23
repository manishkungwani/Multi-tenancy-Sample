using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MultiTenancy.Core;
using MultiTenancy.Tests.Helpers.Controllers;
using StructureMap;
using StructureMap.Configuration.DSL;
using Xunit;
using FooControllerSub = MultiTenancy.Tests.Helpers.Controllers.Sub.FooController;

namespace MultiTenancy.Tests.Core
{
    public class ControllerConventionTests
    {
        [Fact]
        public void ControllerConvention_Process_NonController_IsNotAddedToRegistry()
        { 
            var registry = new Registry();
            new ControllerConvention().Process(typeof(ControllerConvention), registry);
            Assert.Empty(GetControllersFrom(registry));
        }

        [Fact]
        public void ControllerConvention_Process_Controller_WithoutOverrideBaseClass_AddsTypeToRegistry()
        {
            var registry = new Registry();
            new ControllerConvention().Process(typeof(FooController), registry);
            Assert.IsType<FooController>(GetControllersFrom(registry).First());
        }

        [Fact]
        public void ControllerConvention_Process_Controller_WithOverrideBaseClass_AddsTypeToRegistry()
        {
            var registry = new Registry();
            new ControllerConvention().Process(typeof(FooControllerSub), registry);
            Assert.IsType<FooControllerSub>(GetControllersFrom(registry).First());
        }

        [Fact]
        public void ControllerConvention_Process_Controller_WithOverrideBaseClass_AddsSingleTypeToRegistry()
        {
            var registry = new Registry();
            new ControllerConvention().Process(typeof(FooControllerSub), registry);
            Assert.Equal(1, GetControllersFrom(registry).Count);
        }

        [Fact]
        public void ControllerConvention_Process_Controller_WithOverrideBaseClass_RemovesBaseTypeFromRegistry()
        {
            var registry = new Registry();
            new ControllerConvention().Process(typeof(FooController), registry);
            new ControllerConvention().Process(typeof(FooControllerSub), registry);
            Assert.IsType<FooControllerSub>(GetControllersFrom(registry).First());
        }

        [Fact]
        public void ControllerConvention_Process_Controller_WithOverrideBaseClass_AddsOnlyOneValueToControllers()
        {
            var registry = new Registry();
            new ControllerConvention().Process(typeof(FooController), registry);
            new ControllerConvention().Process(typeof(FooControllerSub), registry);
            Assert.Equal(1, GetControllersFrom(registry).Count);
        }

        [Fact]
        public void ControllerConvention_Process_Controller_WithOverrideBaseClass_IgnoresBaseTypeWhenInheritorDefined()
        {
            var registry = new Registry();
            new ControllerConvention().Process(typeof(FooControllerSub), registry);
            new ControllerConvention().Process(typeof(FooController), registry);
            Assert.Equal(1, GetControllersFrom(registry).Count);
        }

        private IList<IController> GetControllersFrom(Registry registry)
        { 
            var container = new Container();
            container.Configure(config => config.AddRegistry(registry));
            return container.GetAllInstances<IController>();
        }
    }
}

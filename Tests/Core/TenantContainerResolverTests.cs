using System;
using System.Web.Routing;
using Moq;
using MultiTenancy.Core;
using StructureMap;
using Xunit;

namespace MultiTenancy.Tests.Core.Specifications
{
    public class TenantContainerResolverTests
    {
        [Fact]
        public void TenantContainerResolver_ThrowsException_GivenNullTenantSelector()
        {
            Assert.Throws<ArgumentNullException>(() => new TenantContainerResolver(null));
        }

        [Fact]
        public void TenantContainerResolver_Resolve_ReturnsContainerFromSelectedTenant()
        {
            var container = new Container();
            var tenant = new Mock<IApplicationTenant>();
            tenant.Setup(x=>x.DependencyContainer).Returns(container);
            var tenantSelector = new Mock<ITenantSelector>();
            tenantSelector.Setup(x => x.Select(It.IsAny<RequestContext>())).Returns(tenant.Object);
            var resolver = new TenantContainerResolver(tenantSelector.Object);
            Assert.Same(container, resolver.Resolve(new RequestContext()));
        }
    }
}

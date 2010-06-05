using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Moq;
using MultiTenancy.Core;
using MultiTenancy.Tests.Helpers;
using Xunit;

namespace MultiTenancy.Tests.Core.Specifications
{
    public class DefaultTenantSelectorTests
    {
        [Fact]
        public void DefaultTenantSelector_Ctr_SetsControllers()
        { 
            var tenants = Enumerable.Empty<IApplicationTenant>();
            var selector = new DefaultTenantSelector(tenants);
            Assert.Same(tenants, selector.Tenants);
        }

        [Fact]
        public void DefaultTenantSelector_Ctr_ThrowsException_GivenNullTenants()
        {
            Assert.Throws<ArgumentNullException>(() => new DefaultTenantSelector(null));
        }

        [Fact]
        public void DefaultTenantSelector_Select_ThrowsException_GivenNullRequestContext()
        {
            Assert.Throws<ArgumentNullException>(() => new DefaultTenantSelector(Enumerable.Empty<IApplicationTenant>()).Select(null));
        }

        [Fact]
        public void DefaultTenantSelector_Select_ReturnsTenantWithSpecifiedBasePath()
        {
            var url = "http://wwww.eagleenvision.net";

            var expected = GenerateTenant(url);
            var tenants = new[] { GenerateTenant(),
                                  GenerateTenant("http://www.google.com", "http://www.yahoo.com"), 
                                  expected };
            var selector = new DefaultTenantSelector(tenants);
            Assert.Same(expected, selector.Select(FakeRequest(url)));
        }

        [Fact]
        public void DefaultTenantSelector_Select_ThrowsException_WhenNoTenantWithPathExists()
        {
            var tenants = new[] { GenerateTenant(),
                                  GenerateTenant("http://www.google.com", "http://www.yahoo.com"), 
                                  GenerateTenant("http://www.eagleenvision.net") };

            Assert.Throws<TenantNotFoundException>(() => new DefaultTenantSelector(tenants).Select(FakeRequest("http://www.foo.com")));
        }

        private RequestContext FakeRequest(string url)
        {
            var mockHttpContext = new Mock<HttpContextBase>();
            var mockHttpRequest = new Mock<HttpRequestBase>();
            mockHttpRequest.MapBaseUrl(url);
            mockHttpContext.Setup(x => x.Request).Returns(mockHttpRequest.Object);
            return new RequestContext { HttpContext = mockHttpContext.Object };
        }

        private IApplicationTenant GenerateTenant(params string[] paths)
        {
            var mock = new Mock<IApplicationTenant>();
            mock.SetupGet(x => x.UrlPaths).Returns(paths);
            return mock.Object;
        }
    }
}

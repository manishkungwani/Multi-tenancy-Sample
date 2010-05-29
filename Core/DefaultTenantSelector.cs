namespace MultiTenancy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Routing;

    /// <summary>
    /// Default tenant selector that will select tenants based on request URL path
    /// </summary>
    public class DefaultTenantSelector : ITenantSelector
    {
        /// <summary>
        /// Initializes a new instance of the DefaultTenantSelector class that selects tenants based on the request URL
        /// </summary>
        /// <param name="tenants">Tenants used by the application</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="tenants"/> is null</exception>
        public DefaultTenantSelector(IEnumerable<IApplicationTenant> tenants)
        {
            Ensure.Argument.NotNull(tenants, "tenants");
            this.Tenants = tenants;
        }

        /// <summary>
        /// Gets the tenants used by the application
        /// </summary>
        public IEnumerable<IApplicationTenant> Tenants { get; private set; }

        /// <summary>
        /// Selects URL based upon their base URL
        /// </summary>
        /// <param name="context">Context of the request</param>
        /// <returns>Application tenant for the request</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="context"/> is null</exception>
        /// <exception cref="MultiTenancy.Core.Specification.TenantNotFoundException">Thrown when a tenant doesn't match the request</exception>
        public IApplicationTenant Select(RequestContext context)
        {
            Ensure.Argument.NotNull(context, "context");

            string baseurl = context.HttpContext.BaseUrl().TrimEnd('/');

            var valid = from tenant in this.Tenants
                        from path in tenant.UrlPaths
                        where path.Trim().TrimEnd('/').Equals(baseurl, StringComparison.OrdinalIgnoreCase)
                        select tenant;

            if (!valid.Any())
                throw new TenantNotFoundException();
            return valid.First();
        }
    }
}

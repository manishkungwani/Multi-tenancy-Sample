namespace MultiTenancy.Core
{
    using System.Collections.Generic;
    using System.Web.Routing;

    /// <summary>
    /// Selector of the application tenant
    /// </summary>
    public interface ITenantSelector
    {
        /// <summary>
        /// Gets all tenants of the application
        /// </summary>
        IEnumerable<IApplicationTenant> Tenants { get; }

        /// <summary>
        /// Selects an application tenant based upon the request
        /// </summary>
        /// <param name="context">Context of the request</param>
        /// <returns>Application tenant when available</returns>
        /// <exception cref="MultiTenancy.Core.Specification.TenantNotFoundException">Thrown when a tenant is not found</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="context"/> is null</exception>
        IApplicationTenant Select(RequestContext context);
    }
}

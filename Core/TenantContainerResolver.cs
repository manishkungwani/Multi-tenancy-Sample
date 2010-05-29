namespace MultiTenancy.Core
{
    using System.Web.Routing;
    using StructureMap;

    /// <summary>
    /// Container resolver that gets a dependency container by a tenant selector
    /// </summary>
    public class TenantContainerResolver : IContainerResolver
    {
        /// <summary>
        /// Tenant selection used to resolve container from tenant
        /// </summary>
        private readonly ITenantSelector tenantSelector;

        /// <summary>
        /// Initializes a new instance of the TenantContainerResolver class that resolves a container by a tenant
        /// </summary>
        /// <param name="tenantSelector">Tenant selector used to resolve container</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="tenantSelector"/> is null</exception>
        public TenantContainerResolver(ITenantSelector tenantSelector)
        {
            Ensure.Argument.NotNull(tenantSelector, "tenantSelector");
            this.tenantSelector = tenantSelector;
        }

        /// <summary>
        /// Resolves the container of the appropriate application tenant
        /// </summary>
        /// <param name="context">Request context used to select the container</param>
        /// <returns>Container from the appropriate tenant</returns>
        public IContainer Resolve(RequestContext context)
        {
            return this.tenantSelector.Select(context).DependencyContainer;
        }
    }
}
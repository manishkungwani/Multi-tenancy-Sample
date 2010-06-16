namespace MultiTenancy.Core
{
    using System.Web.Mvc;

    /// <summary>
    /// View engine that resolves views by the tenant
    /// </summary>
    public class TenantViewEngine : IViewEngine
    {
        /// <summary>
        /// Tenant selector
        /// </summary>
        private readonly ITenantSelector tenantSelector;

        /// <summary>
        /// Initializes a new instance of the TenantViewEngine class that resolves views based upon the tenant for the request
        /// </summary>
        /// <param name="tenantSelector">Tenant selector used to resolve views</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="tenantSelector"/> is null</exception>
        public TenantViewEngine(ITenantSelector tenantSelector)
        {
            Ensure.Argument.NotNull(tenantSelector, "tenantSelector");
            this.tenantSelector = tenantSelector;
        }

        /// <summary>
        /// Finds the specified partial view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="useCache">true to specify that the view engine returns the cached view, if a cached view exists; otherwise, false.</param>
        /// <returns>The partial view.</returns>
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return this.SelectViewEngine(controllerContext).FindPartialView(controllerContext, partialViewName, useCache);
        }

        /// <summary>
        /// Finds the specified view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="masterName">The name of the master.</param>
        /// <param name="useCache">true to specify that the view engine returns the cached view, if a cached view exists; otherwise, false.</param>
        /// <returns>The page view.</returns>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return this.SelectViewEngine(controllerContext).FindView(controllerContext, viewName, masterName, useCache);
        }

        /// <summary>
        /// Releases the specified view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="view">The view to release</param>
        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            this.SelectViewEngine(controllerContext).ReleaseView(controllerContext, view);
        }

        /// <summary>
        /// Selects a view engine by the applicable tenant
        /// </summary>
        /// <param name="controllerContext">Request context</param>
        /// <returns>View engine used to resolve views</returns>
        /// <exception cref="Eagleenvision.ContentFramework.Core.Specification.TenantNotFoundException">Thrown when a tenant is not found</exception>
        protected virtual IViewEngine SelectViewEngine(ControllerContext controllerContext)
        {
            return this.tenantSelector.Select(controllerContext.RequestContext).ViewEngine;
        }
    }
}

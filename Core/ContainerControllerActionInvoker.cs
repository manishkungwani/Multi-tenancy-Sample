namespace MultiTenancy.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Controller Action Invoker that will inject containers when necessary
    /// </summary>
    public class ContainerControllerActionInvoker : ControllerActionInvoker
    {
        /// <summary>
        /// Container resolver used to inject containers into filters
        /// </summary>
        private readonly IContainerResolver containerResolver;

        /// <summary>
        /// Initializes a new instance of the ContainerControllerActionInvoker class
        /// </summary>
        /// <param name="containerResolver">Dependency container resolver used to inject containers when necessary</param>
        public ContainerControllerActionInvoker(IContainerResolver containerResolver)
        {
            Ensure.Argument.NotNull(containerResolver, "containerResolver");
            this.containerResolver = containerResolver;
        }

        /// <summary>
        /// Gets the filters used in the execution
        /// </summary>
        /// <param name="controllerContext">Controller execution context</param>
        /// <param name="actionDescriptor">Action descriptor</param>
        /// <returns>Filters used for action execution</returns>
        protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);
            var requestContext = new RequestContext(controllerContext.HttpContext, controllerContext.RouteData);
            this.InjectContainers(filters.ActionFilters, requestContext);
            this.InjectContainers(filters.AuthorizationFilters, requestContext);
            this.InjectContainers(filters.ExceptionFilters, requestContext);
            this.InjectContainers(filters.ResultFilters, requestContext);
            return filters;
        }

        /// <summary>
        /// Injects containers into filters that need to be injected with a container
        /// </summary>
        /// <param name="values">Filters that possibly need containers</param>
        /// <param name="requestContext">Request context needed for container resolution</param>
        private void InjectContainers(IEnumerable values, RequestContext requestContext)
        {
            values.OfType<IContainerFilter>()
                  .Each(filter => filter.DependencyContainer = this.containerResolver.Resolve(requestContext));
        }
    }
}
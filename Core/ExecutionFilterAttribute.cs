namespace MultiTenancy.Core
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using StructureMap;

    /// <summary>
    /// Attribute for filtering controller acceptance
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class ExecutionFilterAttribute : FilterAttribute, IActionFilter, IContainerFilter
    {
        /// <summary>
        /// Gets or sets the dependency container valid for the context
        /// </summary>
        public IContainer DependencyContainer { get; set; }

        /// <summary>
        /// Placeholder for post-action execution
        /// </summary>
        /// <param name="filterContext">Action Exceution Context</param>
        /// <remarks>No behavior</remarks>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Nothing to execute after action execution
        }

        /// <summary>
        /// Behavior that validates based upon the validator
        /// </summary>
        /// <param name="filterContext">Action executing context</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!this.Validate(this.DependencyContainer, filterContext))
                this.OnInvalid(filterContext);
        }

        /// <summary>
        /// Gets whether this controller is valid for the given context
        /// </summary>
        /// <param name="dependencyContainer">Container for dependencies</param>
        /// <param name="filterContext">Context of the request</param>
        /// <returns>True if the controller is valid, false otherwise</returns>
        public abstract bool Validate(IContainer dependencyContainer, ActionExecutingContext filterContext);

        /// <summary>
        /// Handles invalid executions
        /// </summary>
        /// <param name="filterContext">Action execution context</param>
        protected virtual void OnInvalid(ActionExecutingContext filterContext)
        {
            throw new HttpException(404, "The request cannot be satisfied");
        }
    }
}
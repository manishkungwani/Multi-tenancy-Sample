namespace MultiTenancy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using StructureMap;

    /// <summary>
    /// Attribute for specifying the feature the controller belongs to
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FeatureAttribute : ExecutionFilterAttribute
    {
        /// <summary>
        /// Initializes a new instance of the FeatureAttribute class that specifies the feature 
        /// the controller belongs to
        /// </summary>
        /// <param name="featurePath">Path to the finest subfeature</param>
        public FeatureAttribute(params string[] featurePath)
        {
            Ensure.Argument.NotNull(featurePath, "featurePath");
            this.FeaturePath = featurePath.Where(x => x.IsNotNullOrEmpty());
        }

        /// <summary>
        /// Gets the name of the feature the controller belongs to
        /// </summary>
        public IEnumerable<string> FeaturePath { get; private set; }

        /// <summary>
        /// Gets whether the controller is valid for the execution
        /// </summary>
        /// <param name="dependencyContainer">Dependency container to check for feature registry</param>
        /// <param name="filterContext">Action context for the request</param>
        /// <returns>True if the controller feature is vaild for this controller, false otherwise</returns>
        public override bool Validate(IContainer dependencyContainer, ActionExecutingContext filterContext)
        {
            Ensure.Argument.NotNull(dependencyContainer, "dependencyContainer");

            var registry = dependencyContainer.TryGetInstance<IFeatureRegistry>();

            // if the feature path is empty, the feature path is equal to 
            // the Controller/Action path
            if (!this.FeaturePath.Any())
            {
                this.FeaturePath = new[] 
                { 
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Without("Controller"), 
                    filterContext.ActionDescriptor.ActionName 
                };
            }

            return registry == null || registry.IsEnabled(this.FeaturePath);
        }
    }
}
namespace MultiTenancy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Spark.Web.Mvc;
    using StructureMap;

    /// <summary>
    /// Application tenant that defines default behavior for view engine registration
    /// </summary>
    public abstract class AbstractApplicationTenant : IApplicationTenant
    {
        /// <summary>
        /// Gets the name of the application (company name)
        /// </summary>
        public string ApplicationName { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the JavaScript compilation should be minified
        /// </summary>
        public bool MinifyJavaScript { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the CSS file should be minified
        /// </summary>
        public bool MinifyCSS { get; protected set; }

        /// <summary>
        /// Gets the features used by this tenant
        /// </summary>
        public IFeatureRegistry EnabledFeatures { get; protected set; }

        /// <summary>
        /// Gets the base URL paths the applicaiton should utilize
        /// </summary>
        public IEnumerable<string> UrlPaths { get; protected set; }

        /// <summary>
        /// Gets the default dependency container
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// The returned container should be the same as the
        /// container as the container in the application settings
        /// </remarks>
        public IContainer DependencyContainer { get; protected set; }

        /// <summary>
        /// Gets the view engine used with this tenant
        /// </summary>
        public IViewEngine ViewEngine { get; protected set; }

        /// <summary>
        /// Gets the view engine from a precompiled view file with the same name as the assembly of the tenant type is "Views" appended
        /// </summary>
        /// <returns>View engine for the application tenant</returns>
        protected virtual IViewEngine DetermineViewEngine()
        {
            var factory = new SparkViewFactory();
            var file = GetType().Assembly.CodeBase.Without("file:///").Replace(".dll", ".Views.dll").Replace('/', '\\');
            var assembly = Assembly.LoadFile(file);
            factory.Engine.LoadBatchCompilation(assembly);
            return factory;
        }
    }
}
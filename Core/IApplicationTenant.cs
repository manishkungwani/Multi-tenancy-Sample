namespace MultiTenancy.Core
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using StructureMap;

    /// <summary>
    /// Implementers of this interface must specify semantics of the application
    /// </summary>
    /// <remarks>The assembly of the implementer must contain the necessary pieces of the application</remarks>
    public interface IApplicationTenant
    {
        /// <summary>
        /// Gets the name of the application (company name)
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Gets a value indicating whether the JavaScript compilation should be minified
        /// </summary>
        bool MinifyJavaScript { get; }

        /// <summary>
        /// Gets a value indicating whether the CSS file should be minified
        /// </summary>
        bool MinifyCSS { get; }

        /// <summary>
        /// Gets the features used by this tenant
        /// </summary>
        IFeatureRegistry EnabledFeatures { get; }

        /// <summary>
        /// Gets the base URL paths the applicaiton should utilize
        /// </summary>
        IEnumerable<string> UrlPaths { get; }

        /// <summary>
        /// Gets the default dependency container
        /// </summary>
        /// <remarks>
        ///     The returned container should be the same as the 
        ///     container as the container in the application settings
        /// </remarks>
        IContainer DependencyContainer { get; }

        /// <summary>
        /// Gets the view engine used with this tenant
        /// </summary>
        IViewEngine ViewEngine { get; }
    }
}

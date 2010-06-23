namespace MultiTenancy.Tenants.Sample1
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MultiTenancy.Core;
    using MultiTenancy.Web;
    using StructureMap;

    /// <summary>
    /// Assembly view path settings for compilation
    /// </summary>
    public static class AssemblySettings
    {
        /// <summary>
        /// Embedded assembly view paths stored with corresponding assembly
        /// </summary>
        public static readonly IEnumerable<Tuple<Assembly, string>> AssemblyViewPaths = new Tuple<Assembly, string>[]
        {
            // This assembly's views first
            Tuple.Create(typeof(SampleTenant).Assembly, typeof(SampleTenant).Namespace + ".Views"),

            // "Host" views next
            Tuple.Create(typeof(MvcApplication).Assembly, typeof(MvcApplication).Namespace + ".Views")
        };

        /// <summary>
        /// Forms a dependency container
        /// </summary>
        /// <param name="customExpression">Custom configuration expression</param>
        /// <returns>Container constructed for the application</returns>
        public static IContainer FormContainer(Action<ConfigurationExpression> customExpression = null)
        {
            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(scanner =>
                {
                    scanner.Convention<ControllerConvention>();
                    AssemblySettings.AssemblyViewPaths.Each(path => scanner.Assembly(path.Item1));
                });

                if (customExpression != null)
                    customExpression(config);
            });

            return container;
        }
    }
}
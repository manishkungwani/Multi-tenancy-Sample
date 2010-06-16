namespace MultiTenancy.Tenants.Sample1
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MultiTenancy.Web;

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
    }
}
namespace MultiTenancy.Core
{
    using System;
    using System.Web.Mvc;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Interceptors;

    /// <summary>
    /// Controller convention that will override a controller if the controller has the same name
    /// </summary>
    public class ControllerConvention : IRegistrationConvention
    {
        /// <summary>
        /// Processes types from an assembly for controllers and implements controller overriding behavior
        /// </summary>
        /// <param name="type">Type to process</param>
        /// <param name="registry">Registry on which to register controllers</param>
        public void Process(Type type, Registry registry)
        {
            // ensure the type is not null, registry isn't null and the type is a valid controller
            if (registry == null || !IsValidController(type))
                return;

            var baseClass = type.BaseType;

            // type must inherit from controller with same type name to be overriden.
            // The type should be added as normal
            // NOTE: this can be extended to be resolved by attribute or something of the sort
            if (!IsValidController(baseClass) || !baseClass.Name.Equals(type.Name))
                registry.AddType(typeof(IController), type);
            else
            {
                registry.AddType(typeof(IController), baseClass);
                registry.RegisterInterceptor(new TypeReplacementInterceptor(baseClass, type));
            }
        }

        /// <summary>
        /// Gets whether a type is a valid controller
        /// </summary>
        /// <param name="type">Type to test for controller</param>
        /// <returns>Value indicating whether the type is a controller</returns>
        private static bool IsValidController(Type type)
        {
            return type != null && !type.IsAbstract && typeof(IController).IsAssignableFrom(type) &&
                   type.Name.EndsWith("Controller") && type.IsPublic;
        }

        /// <summary>
        /// Interceptor that will replace an instance of a requested type
        /// </summary>
        private class TypeReplacementInterceptor : TypeInterceptor
        {
            /// <summary>
            /// Type to intercept and replace
            /// </summary>
            private readonly Type typeToReplace;

            /// <summary>
            /// Type used as the replacement
            /// </summary>
            private readonly Type replacementType;

            /// <summary>
            /// Initializes a new instance of the TypeReplacementInterceptor class that will replace an instance
            /// </summary>
            /// <param name="typeToReplace">Requested type to intercept</param>
            /// <param name="replacementType">Type to replace</param>
            /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="typeToReplace"/> or <paramref name="replacementType"/> is null</exception>
            public TypeReplacementInterceptor(Type typeToReplace, Type replacementType)
            {
                Ensure.Argument.NotNull(typeToReplace, "typeToReplace");
                Ensure.Argument.NotNull(replacementType, "replacementType");

                this.typeToReplace = typeToReplace;
                this.replacementType = replacementType;
            }

            /// <summary>
            /// Matches type against requested replacement type
            /// </summary>
            /// <param name="type">Type to test for replacement</param>
            /// <returns>Value indicating whether type should be intercepted</returns>
            public bool MatchesType(Type type)
            {
                return type != null && type.Equals(this.typeToReplace);
            }

            /// <summary>
            /// Replaces an instance with the replacement type
            /// </summary>
            /// <param name="target">Instance already initialized</param>
            /// <param name="context">Context of the interception</param>
            /// <returns>New object that replaces the <paramref name="target"/> with an object with the replacement type</returns>
            public object Process(object target, IContext context)
            {
                // Sanity check: If the context is null, we can't do anything about it!
                if (context == null)
                    return target;
                return context.GetInstance(this.replacementType);
            }
        }
    }

    public class typ : ITypeScanner
    {
        public void Process(Type type, PluginGraph graph)
        {
            graph.InterceptorLibrary.AddInterceptor(
        }
    }

}
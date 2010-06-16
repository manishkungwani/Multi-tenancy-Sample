using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using Spark;
using Spark.FileSystem;
using Spark.Web.Mvc.Install;

namespace MultiTenancy.Tenants.Sample1
{
    [RunInstaller(true)]
    public partial class PostBuildStep : Installer
    {
        public PostBuildStep()
        {
            InitializeComponent();
        }

        private void precompileInstaller1_DescribeBatch(object sender, DescribeBatchEventArgs e)
        {
            // Add controllers from assemblies. 
            // NOTE: This will change to reflect the override model.
            foreach (var viewSetting in AssemblySettings.AssemblyViewPaths)
                e.Batch.FromAssembly(viewSetting.Item1);
        }

        /// <summary>
        /// Gets the spark settings used for view generation
        /// </summary>
        private ISparkSettings Settings
        {
            get
            {
                var settings = new SparkSettings().SetAutomaticEncoding(true)
                                                  .SetDefaultLanguage(LanguageType.CSharp);

                #if DEBUG
                settings.SetDebug(true);
                #else
                settings.SetDebug(false);
                #endif

                // add embedded view folders from settings
                foreach (var viewSetting in AssemblySettings.AssemblyViewPaths)
                    settings.AddViewFolder(ViewFolderType.EmbeddedResource, EmbeddedFor(viewSetting.Item1, viewSetting.Item2));

                return settings;
            }
        }

        /// <summary>
        /// Helper for generating parameters for Embedded Resource ViewFolderType
        /// </summary>
        /// <param name="assembly">Resource assembly</param>
        /// <param name="pathToResources">Path to views (namespaced)</param>
        /// <returns>Parameters for view folder</returns>
        private IDictionary<string, string> EmbeddedFor(Assembly assembly, string pathToResources)
        {
            // new EmbeddedViewFolder(assembly: ____, resourcePath: ____);
            return new Dictionary<string, string>
            { 
                {"assembly", assembly.FullName },
                {"resourcePath", pathToResources}
            };            
        }
    }
}
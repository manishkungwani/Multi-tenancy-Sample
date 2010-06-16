using System.ComponentModel;
using System.Configuration.Install;
using Spark.Web.Mvc.Install;

namespace MultiTenancy.Tenants.Sample1
{
    partial class PostBuildStep
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.precompileInstaller1 = new PrecompileInstaller();
            // 
            // precompileInstaller1
            // 
            this.precompileInstaller1.SettingsInstantiator = () => Settings;
            this.precompileInstaller1.DescribeBatch += new DescribeBatchHandler(precompileInstaller1_DescribeBatch);
            
            // 
            // PostBuildStep
            // 
            this.Installers.AddRange(new Installer[] {
            this.precompileInstaller1});

        }

        #endregion

        private PrecompileInstaller precompileInstaller1;
    }
}
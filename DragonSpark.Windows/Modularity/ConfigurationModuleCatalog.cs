using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Windows.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows.Modularity
{

	/// <summary>
	/// A catalog built from a configuration file.
	/// </summary>
	public class ConfigurationModuleCatalog : ModuleCatalog
    {
        /// <summary>
        /// Builds an instance of ConfigurationModuleCatalog with a <see cref="ConfigurationStore"/> as the default store.
        /// </summary>
        public ConfigurationModuleCatalog()
        {
            this.Store = new ConfigurationStore();
        }

        /// <summary>
        /// Gets or sets the store where the configuration is kept.
        /// </summary>
        public IConfigurationStore Store { get; set; }

        /// <summary>
        /// Loads the catalog from the configuration.
        /// </summary>
        protected override void InnerLoad()
        {
            if (this.Store == null)
            {
                throw new InvalidOperationException(Resources.ConfigurationStoreCannotBeNull);
            }

            this.EnsureModulesDiscovered();
        }

        private static string GetFileAbsoluteUri(string filePath)
        {
	        var uriBuilder = new UriBuilder
	        {
		        Host = string.Empty,
		        Scheme = Uri.UriSchemeFile,
		        Path = Path.GetFullPath( filePath )
	        };
	        var result = uriBuilder.Uri.ToString();
	        return result;
        }

        private void EnsureModulesDiscovered()
        {
            ModulesConfigurationSection section = this.Store.RetrieveModuleConfigurationSection();

            if (section != null)
            {
                foreach (ModuleConfigurationElement element in section.Modules)
                {
                    IList<string> dependencies = new List<string>( element.Dependencies.Cast<ModuleDependencyConfigurationElement>().Select( x => x.ModuleName ) );

                    ModuleInfo moduleInfo = new DynamicModuleInfo(element.ModuleName, element.ModuleType)
                    {
                        Ref = GetFileAbsoluteUri(element.AssemblyFile),
                        InitializationMode = element.StartupLoaded ? InitializationMode.WhenAvailable : InitializationMode.OnDemand
                    };
                    moduleInfo.DependsOn.AddRange(dependencies.ToArray());
                    AddModule(moduleInfo);
                }
            }
        }
    }
}

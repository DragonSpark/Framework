using System;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Modularity
{
    public static class ModuleCatalogExtensions
    {
        /// <summary>
        /// Adds a groupless <see cref="ModuleInfo"/> to the catalog.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="moduleType"><see cref="Type"/> of the module to be added.</param>
        /// <param name="initializationMode">Stage on which the module to be added will be initialized.</param>
        /// <param name="dependsOn">Collection of module names (<see cref="ModuleInfo.ModuleName"/>) of the modules on which the module to be added logically depends on.</param>
        /// <returns>The same <see cref="ModuleCatalog"/> instance with the added module.</returns>
        public static ModuleCatalog AddModule(this ModuleCatalog @this, [Required]Type moduleType, InitializationMode initializationMode, params string[] dependsOn)
        {
            return @this.AddModule(moduleType.Name, moduleType.AssemblyQualifiedName, initializationMode, dependsOn);
        }

        /// <summary>
        /// Adds a groupless <see cref="ModuleInfo"/> to the catalog.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="moduleName">Name of the module to be added.</param>
        /// <param name="moduleType"><see cref="Type"/> of the module to be added.</param>
        /// <param name="initializationMode">Stage on which the module to be added will be initialized.</param>
        /// <param name="dependsOn">Collection of module names (<see cref="ModuleInfo.ModuleName"/>) of the modules on which the module to be added logically depends on.</param>
        /// <returns>The same <see cref="ModuleCatalog"/> instance with the added module.</returns>
        public static ModuleCatalog AddModule(this ModuleCatalog @this, string moduleName, string moduleType, InitializationMode initializationMode, params string[] dependsOn)
        {
            return @this.AddModule(moduleName, moduleType, null, initializationMode, dependsOn);
        }

        /// <summary>
        /// Adds a groupless <see cref="ModuleInfo"/> to the catalog.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="moduleName">Name of the module to be added.</param>
        /// <param name="moduleType"><see cref="Type"/> of the module to be added.</param>
        /// <param name="refValue">Reference to the location of the module to be added assembly.</param>
        /// <param name="initializationMode">Stage on which the module to be added will be initialized.</param>
        /// <param name="dependsOn">Collection of module names (<see cref="ModuleInfo.ModuleName"/>) of the modules on which the module to be added logically depends on.</param>
        /// <returns>The same <see cref="ModuleCatalog"/> instance with the added module.</returns>
        public static ModuleCatalog AddModule(this ModuleCatalog @this, [Required]string moduleName, [Required]string moduleType, string refValue, InitializationMode initializationMode, params string[] dependsOn)
        {
            var moduleInfo = new DynamicModuleInfo(moduleName, moduleType);
            moduleInfo.DependsOn.AddRange(dependsOn);
            moduleInfo.InitializationMode = initializationMode;
            moduleInfo.Ref = refValue;
            @this.Items.Add(moduleInfo);
            return @this;
        }

        /// <summary>
        /// Creates and adds a <see cref="ModuleInfoGroup"/> to the catalog.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="initializationMode">Stage on which the module group to be added will be initialized.</param>
        /// <param name="refValue">Reference to the location of the module group to be added.</param>
        /// <param name="moduleInfos">Collection of <see cref="ModuleInfo"/> included in the group.</param>
        /// <returns><see cref="ModuleCatalog"/> with the added module group.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
        public static  ModuleCatalog AddGroup(this ModuleCatalog @this, InitializationMode initializationMode, string refValue, [Required]params ModuleInfo[] moduleInfos)
        {
            var newGroup = new DynamicModuleInfoGroup { InitializationMode = initializationMode, Ref = refValue };

            foreach (ModuleInfo info in moduleInfos)
            {
                newGroup.Add(info);
            }

            @this.Items.Add(newGroup);

            return @this;
        }
    }
}
using System;

namespace Prism.Modularity
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
        public static ModuleCatalog AddModule(this ModuleCatalog @this, Type moduleType, InitializationMode initializationMode, params string[] dependsOn)
        {
            if (moduleType == null) throw new System.ArgumentNullException("moduleType");
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
        public static ModuleCatalog AddModule(this ModuleCatalog @this, string moduleName, string moduleType, string refValue, InitializationMode initializationMode, params string[] dependsOn)
        {
            if (moduleName == null)
            {
                throw new ArgumentNullException("moduleName");
            }

            if (moduleType == null)
            {
                throw new ArgumentNullException("moduleType");
            }

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
        public static  ModuleCatalog AddGroup(this ModuleCatalog @this, InitializationMode initializationMode, string refValue, params ModuleInfo[] moduleInfos)
        {
            if (moduleInfos == null) throw new System.ArgumentNullException("moduleInfos");

            var newGroup = new DynamicModuleInfoGroup { InitializationMode = initializationMode, Ref = refValue };

            foreach (ModuleInfo info in moduleInfos)
            {
                newGroup.Add(info);
            }

            @this.Items.Add(newGroup);

            return @this;
        }
    }

    public class DynamicModuleInfoGroup : ModuleInfoGroup
    {
        /// <summary>
        /// Gets or sets the <see cref="DynamicModuleInfo.InitializationMode"/> for the whole group. Any <see cref="ModuleInfo"/> classes that are
        /// added after setting this value will also get this <see cref="InitializationMode"/>.
        /// </summary>
        /// <see cref="DynamicModuleInfo.InitializationMode"/>
        /// <value>The initialization mode.</value>
        public InitializationMode InitializationMode { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DynamicModuleInfo.Ref"/> value for the whole group. Any <see cref="DynamicModuleInfo"/> classes that are
        /// added after setting this value will also get this <see cref="Ref"/>.
        /// 
        /// The ref value will also be used by the <see cref="IModuleManager"/> to determine which  <see cref="IModuleTypeLoader"/> to use. 
        /// For example, using an "file://" prefix with a valid URL will cause the FileModuleTypeLoader to be used
        /// (Only available in the desktop version of CAL).
        /// </summary>
        /// <see cref="DynamicModuleInfo.Ref"/>
        /// <value>The ref value that will be used.</value>
        public string Ref { get; set; }

        protected override void ForwardValues( ModuleInfo moduleInfo )
        {
            base.ForwardValues( moduleInfo );

            var dynamicModuleInfo = moduleInfo as DynamicModuleInfo;
            if ( dynamicModuleInfo != null )
            {
                if (dynamicModuleInfo.Ref == null)
                {
                    dynamicModuleInfo.Ref = this.Ref;
                }

                if (dynamicModuleInfo.InitializationMode == InitializationMode.WhenAvailable && this.InitializationMode != InitializationMode.WhenAvailable)
                {
                    dynamicModuleInfo.InitializationMode = this.InitializationMode;
                }
            }
        }
    }
}
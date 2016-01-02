

using System;

namespace DragonSpark.Modularity
{
    /// <summary>
    /// Defines the interface for the service that will retrieve and initialize the application's modules.
    /// </summary>
    public interface IModuleManager
    {
        /// <summary>
        /// Initializes the modules marked as <see cref="ModuleInfo.IsAvailable"/> on the <see cref="ModuleCatalog"/>.
        /// </summary>
        void Run();

        /// <summary>
        /// Loads and initializes the module on the <see cref="ModuleCatalog"/> with the name <paramref name="moduleName"/>.
        /// </summary>
        /// <param name="moduleName">Name of the module requested for initialization.</param>
        void LoadModule(string moduleName);
       
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Prism.Modularity
{
    public abstract class AssemblyModuleCatalog : ModuleCatalog
    {
        readonly IAssemblyProvider provider;
        readonly IModuleInfoBuilder builder;

        protected AssemblyModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder )
        {
            this.provider = provider;
            this.builder = builder;
        }

        protected IModuleInfoBuilder Builder
        {
            get { return builder; }
        }

        /// <summary>
        ///     Drives the main logic of building the child domain and searching for the assemblies.
        /// </summary>
        protected override void InnerLoad()
        {
            var assemblies = GetAssemblies();

            var items = GetModuleInfos( assemblies );
           
            Items.AddRange( items );
        }

        protected virtual IEnumerable<ModuleInfo> GetModuleInfos( IEnumerable<Assembly> assemblies )
        {
            var info = typeof(IModule).GetTypeInfo();
            var result = assemblies.SelectMany( assembly => assembly.ExportedTypes.Where( type => info.IsAssignableFrom( type.GetTypeInfo() ) ) )
                .Select( Builder.CreateModuleInfo )
                .ToArray();
            return result;
        }

        protected virtual IEnumerable<Assembly> GetAssemblies()
        {
            var result = provider.GetAssemblies();
            return result;
        }
    }
}
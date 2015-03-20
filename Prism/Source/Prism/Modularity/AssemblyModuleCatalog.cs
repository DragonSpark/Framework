using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Prism.Modularity
{
    public abstract class AssemblyModuleCatalog : ModuleCatalog
    {
        readonly IModuleInfoBuilder builder;

        protected AssemblyModuleCatalog( IModuleInfoBuilder builder )
        {
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

        protected abstract IEnumerable<Assembly> GetAssemblies();
    }
}
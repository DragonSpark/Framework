using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Modularity
{
	public abstract class AssemblyModuleCatalog : ModuleCatalog
	{
		readonly IAssemblyProvider provider;

		protected AssemblyModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder )
		{
			this.provider = provider;
			Builder = builder;
		}

		protected IModuleInfoBuilder Builder { get; }

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
			var info = typeof(IModule);
			var result = assemblies.Except( info.Assembly().Append() ).SelectMany( assembly => assembly.ExportedTypes.Where( info.CanActivate ) )
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
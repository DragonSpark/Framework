using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Type = System.Type;

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

		static bool CanLocate<T>( Type type )
		{
			var info = typeof(T).GetTypeInfo();
			var result = ( info.IsInterface || !info.IsAbstract ) && info.Adapt().IsAssignableFrom( type );
			return result;
		}

		protected virtual IEnumerable<ModuleInfo> GetModuleInfos( IEnumerable<Assembly> assemblies )
		{
			var info = typeof(IModule);
			var result = assemblies.Except( info.Assembly().ToItem() ).SelectMany( assembly => assembly.ExportedTypes.Where( CanLocate<IModule> ) )
				.Select( Builder.CreateModuleInfo )
				.ToArray();
			return result;
		}

		protected virtual IEnumerable<Assembly> GetAssemblies()
		{
			var result = provider.Create();
			return result;
		}
	}
}
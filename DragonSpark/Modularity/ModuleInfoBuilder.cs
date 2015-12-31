using DragonSpark.Extensions;
using System;

namespace DragonSpark.Modularity
{
	[Serializable]
	public class ModuleInfoBuilder : IModuleInfoBuilder
	{
		/*public static ModuleInfoBuilder Instance { get; } = new ModuleInfoBuilder();*/

		public ModuleInfoBuilder() : this( AttributeDataProvider.Instance )
		{}

		public ModuleInfoBuilder( IAttributeDataProvider provider )
		{
			Provider = provider;
		}

		protected IAttributeDataProvider Provider { get; }

		void Apply( ModuleInfo result, Type type )
		{
			var dependsOn = Provider.GetAll<string>( typeof(ModuleDependencyAttribute), type, nameof(ModuleDependencyAttribute.ModuleName) );
			result.DependsOn.AddRange( dependsOn );
		}

		public ModuleInfo CreateModuleInfo(Type type)
		{
			string moduleName = Provider.Get<string>( typeof(ModuleAttribute), type, nameof(ModuleDependencyAttribute.ModuleName) ) ?? type.Name;
			var result = Create( type, moduleName, type.AssemblyQualifiedName );
			Apply( result, type );
			return result;
		}

		protected virtual ModuleInfo Create( Type host, string moduleName, string assemblyQualifiedName )
		{
			var result = new ModuleInfo( moduleName, assemblyQualifiedName );
			return result;
		}
	}
}
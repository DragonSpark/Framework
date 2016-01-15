using DragonSpark.Modularity;
using System;

namespace DragonSpark.Windows.Modularity
{
	// [Serializable]
	public class ModuleInfoBuilder : DragonSpark.Modularity.ModuleInfoBuilder
	{
		public static ModuleInfoBuilder Instance { get; } = new ModuleInfoBuilder();

		public ModuleInfoBuilder() : this( CustomAttributeDataProvider.Instance )
		{}

		public ModuleInfoBuilder( IAttributeDataProvider provider ) : base( provider )
		{}

		protected override ModuleInfo Create( Type host, string moduleName, string assemblyQualifiedName )
		{
			var result = new DynamicModuleInfo( moduleName, assemblyQualifiedName )
			{
				InitializationMode = Provider.Get<bool>( typeof(DynamicModuleAttribute), host, "OnDemand" )
					? InitializationMode.OnDemand
					: InitializationMode.WhenAvailable,
				Ref = host.Assembly.CodeBase
			};
			return result;
		}
	}
}
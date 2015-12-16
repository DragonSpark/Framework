using DragonSpark.Modularity;
using System;

namespace DragonSpark.Windows.Modularity
{
	[Serializable]
	public class DynamicModuleInfoBuilder : ModuleInfoBuilder
	{
		public static DynamicModuleInfoBuilder Instance { get; } = new DynamicModuleInfoBuilder();

		public DynamicModuleInfoBuilder() : this( CustomAttributeDataProvider.Instance )
		{}

		public DynamicModuleInfoBuilder( IAttributeDataProvider provider ) : base( provider )
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
using DragonSpark.Modularity;
using DragonSpark.Runtime;
using DragonSpark.Windows.Modularity;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows
{
	public class AssemblyModuleCatalog : DragonSpark.Modularity.AssemblyModuleCatalog
	{
		public AssemblyModuleCatalog() : this( FilteredAssemblyProvider.Instance, new DynamicModuleInfoBuilder() )
		{}

		public AssemblyModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder ) : base( provider, builder )
		{}
	}
}
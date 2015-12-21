using DragonSpark.Modularity;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Modularity;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows
{
	[Register]
	public class AssemblyModuleCatalog : DragonSpark.Modularity.AssemblyModuleCatalog
	{
		public static AssemblyModuleCatalog Instance { get; } = new AssemblyModuleCatalog();

		public AssemblyModuleCatalog() : this( AssemblyProvider.Instance, DynamicModuleInfoBuilder.Instance )
		{}

		public AssemblyModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder ) : base( provider, builder )
		{}
	}
}
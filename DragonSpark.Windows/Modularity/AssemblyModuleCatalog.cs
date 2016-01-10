using DragonSpark.Modularity;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Modularity
{
	[Register.Type]
	public class AssemblyModuleCatalog : DragonSpark.Modularity.AssemblyModuleCatalog
	{
		public AssemblyModuleCatalog( IAssemblyProvider provider, IModuleInfoBuilder builder ) : base( provider, builder )
		{}
	}
}
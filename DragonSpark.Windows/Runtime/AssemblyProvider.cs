using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : FilteredAssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		public AssemblyProvider() : this( FileSystemAssemblyProvider.Instance )
		{}

		public AssemblyProvider( IAssemblyProvider provider ) : base( provider, FilteredAssemblyExpressionFactory.Instance.Create() )
		{}
	}
}
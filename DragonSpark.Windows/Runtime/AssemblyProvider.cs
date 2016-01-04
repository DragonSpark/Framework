using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : AggregateAssemblyFactory
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		public AssemblyProvider() : this( FileSystemAssemblyProvider.Instance )
		{}

		public AssemblyProvider( IAssemblyProvider provider ) : base( provider.Create, ApplicationAssemblyTransformer.Instance.Create )
		{}
	}
}
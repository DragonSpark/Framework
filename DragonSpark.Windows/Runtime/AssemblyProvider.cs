using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : AggregateAssemblyFactory
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		public AssemblyProvider() : base( FileSystemAssemblyProvider.Instance.Create, ApplicationAssemblyTransformer.Instance.Create )
		{}
	}
}
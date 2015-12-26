using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : FilteredAssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		public AssemblyProvider() : this( FileSystemAssemblyProvider.Instance )
		{}

		public AssemblyProvider( IAssemblyProvider provider ) : base( provider )
		{}

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var result = base.DetermineCoreAssemblies().Append( assembly, typeof(AssemblyProvider).Assembly );
			return result;
		}
	}
}
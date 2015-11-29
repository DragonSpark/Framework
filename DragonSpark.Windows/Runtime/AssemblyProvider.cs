using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : FilteredAssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		public AssemblyProvider() : base( FileSystemAssemblyProvider.Instance )
		{}

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var result = base.DetermineCoreAssemblies().Append( assembly, typeof(AssemblyProvider).Assembly );
			return result;
		}
	}
}
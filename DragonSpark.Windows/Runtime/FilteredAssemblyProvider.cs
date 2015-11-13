using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Windows.Runtime
{
	public class FilteredAssemblyProvider : FilteredAssemblyProviderBase
	{
		public static FilteredAssemblyProvider Instance { get; } = new FilteredAssemblyProvider();

		public FilteredAssemblyProvider() : base( AssemblyProvider.Instance )
		{}

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var result = base.DetermineCoreAssemblies().Append( Assembly.GetEntryAssembly(), typeof(FilteredAssemblyProvider).Assembly );
			return result;
		}
	}
}
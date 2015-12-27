using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Runtime
{
	public class FilteredAssemblyExpressionFactory : TypeSystem.FilteredAssemblyExpressionFactory
	{
		public new static FilteredAssemblyExpressionFactory Instance { get; } = new FilteredAssemblyExpressionFactory();

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var result = base.DetermineCoreAssemblies().Append( assembly, typeof(AssemblyProvider).Assembly );
			return result;
		}
	}
}
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Windows.Runtime
{
	[RegisterFactoryForResult]
	public class FilteredAssemblyExpressionFactory : TypeSystem.FilteredAssemblyExpressionFactory
	{
		public new static FilteredAssemblyExpressionFactory Instance { get; } = new FilteredAssemblyExpressionFactory();

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var result = base.DetermineCoreAssemblies().Append( assembly, GetType().Assembly );
			return result;
		}
	}
}
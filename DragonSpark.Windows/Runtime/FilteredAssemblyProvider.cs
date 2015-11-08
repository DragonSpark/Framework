using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;

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

	public class AssemblyProvider : AssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		protected override Assembly[] DetermineAll()
		{
			var result = AllClasses.FromAssembliesInBasePath( includeUnityAssemblies: true )
				.Where( x => x.Namespace != null )
				.GroupBy( type => type.Assembly )
				.Select( types => types.Key ).ToArray();
			return result;
		}
	}
}
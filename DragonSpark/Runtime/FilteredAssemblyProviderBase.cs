using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Setup;

namespace DragonSpark.Runtime
{
	public abstract class FilteredAssemblyProviderBase : AssemblyProviderBase
	{
		readonly IAssemblyProvider provider;
		readonly Func<Assembly, bool> filter;

		readonly Lazy<string[]> namespaces;

		protected FilteredAssemblyProviderBase( IAssemblyProvider provider ) : this( provider, null )
		{}

		protected FilteredAssemblyProviderBase( IAssemblyProvider provider, Func<Assembly, bool> filter )
		{
			this.provider = provider;
			namespaces = new Lazy<string[]>( DetermineNamespaces );
			this.filter = filter ?? ( assembly => assembly.IsDefined( typeof(RegistrationAttribute) ) || namespaces.Value.Any( s => assembly.GetName().Name.StartsWith( s ) ) );
		}

		protected override Assembly[] DetermineAll()
		{
			var result = provider.GetAssemblies().Where( filter ).ToArray();
			return result;
		}

		protected virtual string[] DetermineNamespaces()
		{
			var result = DetermineCoreAssemblies().Distinct().Select( assembly => assembly.GetRootNamespace() ).Distinct().ToArray();
			return result;
		}

		protected virtual IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			yield return typeof(FilteredAssemblyProviderBase).GetTypeInfo().Assembly;
		}
	}
}
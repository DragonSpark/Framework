using DragonSpark.Extensions;
using DragonSpark.Setup;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation
{
	public abstract class AssemblyProvider : AssemblyProviderBase
	{
		readonly IAssemblyProvider provider;
		readonly Func<Assembly, bool> filter;

		readonly Lazy<string[]> namespaces;

		protected AssemblyProvider( IAssemblyProvider provider ) : this( provider, null )
		{}

		protected AssemblyProvider( IAssemblyProvider provider, Func<Assembly, bool> filter )
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
			var result = DetermineCoreAssemblies().Select( assembly => assembly.GetRootNamespace() ).Distinct().ToArray();
			return result;
		}

		protected virtual IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			yield return typeof(AssemblyProvider).GetTypeInfo().Assembly;
		}
	}
}
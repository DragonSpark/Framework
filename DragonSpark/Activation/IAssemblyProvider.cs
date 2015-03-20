using DragonSpark.Extensions;
using DragonSpark.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Activation
{
	public interface IAssemblyProvider
	{
		Assembly[] GetAssemblies();
	}

	public abstract class AssemblyProvider : IAssemblyProvider
	{
		readonly Func<Assembly, bool> filter;

		readonly Lazy<string[]> namespaces;

		readonly Lazy<Assembly[]> all;

		protected AssemblyProvider() : this( null )
		{}

		protected AssemblyProvider( Func<Assembly, bool> filter )
		{
			namespaces = new Lazy<string[]>( DetermineNamespaces );
			this.filter = filter ?? ( assembly => assembly.IsDefined( typeof(RegistrationAttribute) ) || namespaces.Value.Any( s => assembly.GetName().Name.StartsWith( s ) ) );
			all = new Lazy<Assembly[]>( DetermineAllAssemblies );
		}

		protected virtual string[] DetermineNamespaces()
		{
			var result = DetermineCoreAssemblies().Select( assembly => assembly.GetRootNamespace() ).Distinct().ToArray();
			return result;
		}

		protected virtual IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			yield return typeof(IAssemblyProvider).GetTypeInfo().Assembly;
		}

		protected abstract Assembly[] DetermineAllAssemblies();
		
		public Assembly[] GetAssemblies()
		{
			var result = all.Value.Where( filter ).ToArray();
			return result;
		}
	}
}
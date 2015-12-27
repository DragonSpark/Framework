using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public class FilteredAssemblyExpressionFactory : FactoryBase<Func<Assembly, bool>>
	{
		public static FilteredAssemblyExpressionFactory Instance { get; } = new FilteredAssemblyExpressionFactory();

		readonly Lazy<string[]> namespaces;

		public FilteredAssemblyExpressionFactory()
		{
			namespaces = new Lazy<string[]>( DetermineNamespaces );
		}

		protected override Func<Assembly, bool> CreateItem()
		{
			var result = new Func<Assembly, bool>( assembly => assembly.IsDefined( typeof(RegistrationAttribute) ) || namespaces.Value.Any( s => assembly.GetName().Name.StartsWith( s ) ) );
			return result;
		}

		protected virtual string[] DetermineNamespaces()
		{
			var result = DetermineCoreAssemblies().NotNull().Distinct().Select( assembly => assembly.GetRootNamespace() ).Distinct().ToArray();
			return result;
		}

		protected virtual IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			yield return typeof(FilteredAssemblyProviderBase).Assembly();
		}
	}

	public abstract class FilteredAssemblyProviderBase : AssemblyProviderBase
	{
		readonly IAssemblyProvider provider;
		readonly Func<Assembly, bool> filter;

		protected FilteredAssemblyProviderBase( IAssemblyProvider provider ) : this( provider, FilteredAssemblyExpressionFactory.Instance.Create() )
		{}

		protected FilteredAssemblyProviderBase( IAssemblyProvider provider, Func<Assembly, bool> filter )
		{
			this.provider = provider;
			this.filter = filter;
		}

		protected override Assembly[] DetermineAll()
		{
			var result = provider.GetAssemblies().Where( filter ).ToArray();
			return result;
		}
	}
}
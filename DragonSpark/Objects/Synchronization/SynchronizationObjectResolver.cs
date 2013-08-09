using System;
using DragonSpark.Objects;

namespace DragonSpark.Objects.Synchronization
{
	public class SynchronizationObjectResolver : IObjectResolver
	{
		public event EventHandler<ObjectResolvedEventArgs> Resolved = delegate { };

		readonly SynchronizationContainer container;
		readonly string mappingName;
		readonly bool includeBasePolicies;
		readonly IObjectResolver resolver;

		public SynchronizationObjectResolver( SynchronizationContainer container, IObjectResolver resolver ) : this( container, resolver, null )
		{}

		public SynchronizationObjectResolver( SynchronizationContainer container, IObjectResolver resolver, string mappingName ) : this( container, resolver, mappingName, true )
		{}

		public SynchronizationObjectResolver( SynchronizationContainer container, IObjectResolver resolver, string mappingName, bool includeBasePolicies )
		{
			this.container = container;
			this.mappingName = mappingName;
			this.includeBasePolicies = includeBasePolicies;
			this.resolver = resolver;
			resolver.Resolved += ResolverResolved;
		}

		void ResolverResolved(object sender, ObjectResolvedEventArgs e)
		{
			container.Synchronize( e.Source, e.Target, MappingName, IncludeBasePolicies );
			Resolved( this, e );
		}

		public bool IncludeBasePolicies
		{
			get { return includeBasePolicies; }
		}

		public string MappingName
		{
			get { return mappingName; }
		}

		public SynchronizationContainer Container
		{
			get { return container; }
		}

		public object Resolve( object key )
		{
			var result = resolver.Resolve( key );
			return result;
		}
	}
}
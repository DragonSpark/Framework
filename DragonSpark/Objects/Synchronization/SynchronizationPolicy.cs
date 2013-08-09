using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Objects.Synchronization
{
	public class SynchronizationPolicy : ISynchronizationPolicy
	{
        readonly static ReflectedPropertyMappingExpressionResolver ReflectedResolver = new ReflectedPropertyMappingExpressionResolver();

		readonly IEnumerable<ISynchronizationExpressionResolver> resolvers;
		readonly SynchronizationKey key;

		public SynchronizationPolicy( SynchronizationKey key ) : this( key, null )
		{}

		public SynchronizationPolicy( SynchronizationKey key, params ISynchronizationExpressionResolver[] resolvers )
		{
			this.resolvers = ReflectedResolver.ToEnumerable<ISynchronizationExpressionResolver>().Concat( resolvers ?? Enumerable.Empty<ISynchronizationExpressionResolver>() );
			this.key = key;
		}

		public IEnumerable<ISynchronizationExpressionResolver> Resolvers
		{
			get { return resolvers; }
		}

		public SynchronizationKey Key
		{
			get { return key; }
		}

	}
}
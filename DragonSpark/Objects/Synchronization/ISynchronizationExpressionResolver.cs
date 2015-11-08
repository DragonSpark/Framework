using System.Collections.Generic;

namespace DragonSpark.Objects.Synchronization
{
	public interface ISynchronizationExpressionResolver
	{
		IEnumerable<ISynchronizationContext> Resolve( SynchronizationKey key );
	}
}
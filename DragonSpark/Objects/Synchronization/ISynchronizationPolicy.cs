using System.Collections.Generic;

namespace DragonSpark.Objects.Synchronization
{
	public interface ISynchronizationPolicy
	{
		SynchronizationKey Key { get; }
		/*IEnumerable<ISynchronizationExpressionResolver> ResolveContainers( SynchronizationKey key );*/
		IEnumerable<ISynchronizationExpressionResolver> Resolvers { get; }
	}
}
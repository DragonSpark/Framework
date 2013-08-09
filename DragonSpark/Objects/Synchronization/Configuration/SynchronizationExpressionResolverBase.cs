using DragonSpark.Configuration;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	public abstract class SynchronizationExpressionResolverBase : IInstanceSource<ISynchronizationExpressionResolver>
	{
		public ISynchronizationExpressionResolver Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	ISynchronizationExpressionResolver instance;

		protected abstract ISynchronizationExpressionResolver Create();
	}
}
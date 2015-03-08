using DragonSpark.Configuration;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	public abstract class SynchronizationPolicyBase : IInstanceSource<ISynchronizationPolicy>
	{
		public ISynchronizationPolicy Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	ISynchronizationPolicy instance;

		protected abstract ISynchronizationPolicy Create();
	}
}
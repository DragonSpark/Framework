using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class PolicyAwareEach<T> : PolicyAware<T> where T : class
{
	public PolicyAwareEach(IStopAware<T> previous) : base(previous, DurableConnectionPolicy.Default) {}
}
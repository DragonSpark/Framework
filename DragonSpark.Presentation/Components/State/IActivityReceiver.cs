using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Sequences.Collections;

namespace DragonSpark.Presentation.Components.State;

public interface IActivityReceiver : IOperation<ActivityReceiverState>,
									 IResulting<ActivityReceiverState?>,
									 IMembership<IRenderAware>
{
	bool Active { get; }
}
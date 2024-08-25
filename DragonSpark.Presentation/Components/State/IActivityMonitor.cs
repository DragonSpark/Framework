using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences.Collections;

namespace DragonSpark.Presentation.Components.State;

public interface IActivityMonitor : IMembership<IActivityReceiver>, IOperation
{
	bool Active { get; }
}
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.State;

public interface IActivityMonitor : ICommand<IActivityReceiver>, IOperation
{
	bool Active { get; }
}
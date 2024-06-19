using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.State;

public interface IUpdateActivityReceiver : IAssigning<object, ActivityReaderInstance>, IOperation<object>;

// TODO

public readonly record struct ActivityReaderInstance(object Instance, ActivityReceiverInput Input)
{
	public ActivityReaderInstance(object Instance) : this(Instance, ActivityReceiverInput.Default) {}
}
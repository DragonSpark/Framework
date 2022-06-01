using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.State;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActivityAwareActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly ICommand<T> _store;

	public ActivityAwareActiveContent(IActiveContent<T> previous, object receiver)
		: this(previous, previous.Monitor, new ActivityAwareResult<T>(previous, receiver)) {}

	public ActivityAwareActiveContent(ICommand<T> store, IUpdateMonitor refresh, IResulting<T?> resulting)
		: base(resulting)
	{
		_store  = store;
		Monitor = refresh;
	}

	public IUpdateMonitor Monitor { get; }

	public void Execute(T parameter)
	{
		_store.Execute(parameter);
	}
}
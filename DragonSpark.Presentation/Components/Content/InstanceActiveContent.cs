using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Content;

sealed class InstanceActiveContent<T> : Deferring<T?>, IActiveContent<T>
{
	readonly ICommand<T> _store;

	public InstanceActiveContent(IActiveContent<T> previous) : this(previous, previous, UpdateMonitor.Default) {}

	public InstanceActiveContent(ICommand<T> store, IActiveContent<T> previous, IUpdateMonitor refresh)
		: base(A.Result(previous).ToDelegate())
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
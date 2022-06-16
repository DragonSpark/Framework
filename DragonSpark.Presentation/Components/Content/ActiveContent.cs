using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly IMutable<T?> _store;

	public ActiveContent(IResulting<T?> content) : this(content, new Variable<T>(), new Variable<int>()) {}

	public ActiveContent(IResulting<T?> content, IMutable<T?> store, IMutable<int> counts)
		: this(content, new VisitedAwareVariable<T?>(store, counts), counts) {}

	public ActiveContent(IResulting<T?> result, IMutationAware<T?> store, IMutable<int> counts)
		: this(store, new Storing<T?>(store, result), counts) {}

	public ActiveContent(IMutable<T?> store, IResulting<T?> result, IMutable<int> counts)
		: this(store, result, new UpdateMonitor<T?>(result, counts)) {}

	public ActiveContent(IMutable<T?> store, IResulting<T?> result, IUpdateMonitor monitor) : base(result)
	{
		_store  = store;
		Monitor = monitor;
	}

	public IUpdateMonitor Monitor { get; }

	public void Execute(T parameter)
	{
		_store.Execute(parameter);
	}

	public void Dispose() {}
}
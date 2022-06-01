using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	readonly IMutable<T?> _store;

	public ActiveContent(Func<ValueTask<T?>> content) : this(content, new Variable<T>(), new Variable<int>()) {}

	public ActiveContent(Func<ValueTask<T?>> content, IMutable<T?> store, IMutable<int> counts)
		: this(content.Start().Out(), new VisitedAwareVariable<T?>(store, counts), counts) {}

	public ActiveContent(IResulting<T?> result, IMutationAware<T?> store, IMutable<int> counts)
		: this(store, new Storing<T?>(store, result), counts) {}

	public ActiveContent(IMutable<T?> store, IResulting<T?> result, IMutable<int> counts)
		: this(store, result, new RefreshActiveContent<T?>(result, counts)) {}

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
}
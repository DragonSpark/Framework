using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	public ActiveContent(Func<ValueTask<T?>> content) : this(content, new Variable<T>(), new Variable<int>()) {}

	public ActiveContent(Func<ValueTask<T?>> content, IMutable<T?> store, IMutable<int> counts)
		: this(content.Start().Out(), new VisitedAwareVariable<T?>(store, counts), counts) {}

	public ActiveContent(IResulting<T?> result, IMutationAware<T?> store, IMutable<int> counts)
		: this(new Storing<T?>(store, result), counts) {}

	public ActiveContent(IResulting<T?> result, IMutable<int> counts)
		: this(result, new RefreshActiveContent<T?>(result, counts)) {}

	public ActiveContent(IResulting<T?> result, IOperation<Action> refresh) : base(result) => Refresh = refresh;

	public IOperation<Action> Refresh { get; }
}

// TODO
/*
sealed class ActiveContentStore<T> : Storing<T?>
{
	readonly IMutable<T?> _store;

	public ActiveContentStore(Func<ValueTask<T?>> content, IMutable<bool> @switch, IMutable<T?> mutable)
		: this(content, new SwitchAwareVariable<T>(@switch, mutable)) {}

	public ActiveContentStore(Func<ValueTask<T?>> content, IMutationAware<T?> store)
		: base(store, content.Start().Get())
		=> _store = store;

	public void Execute(None parameter)
	{
		_store.Execute(default);
	}
}
*/

sealed class RefreshActiveContent<T> : IOperation<Action>
{
	readonly IResulting<T> _result;
	readonly IMutable<int> _counts;

	public RefreshActiveContent(IResulting<T> result, IMutable<int> counts)
	{
		_result = result;
		_counts = counts;
	}

	public async ValueTask Get(Action parameter)
	{
		_counts.Execute(0);
		var result = _result.Get();
		await result;
		parameter();
	}
}
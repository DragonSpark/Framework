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

	public ActiveContent(IResulting<T?> result, IRequiresUpdate refresh) : base(result) => Refresh = refresh;

	public IRequiresUpdate Refresh { get; }
}

// TODO

public interface IRequiresUpdate : IOperation<Action>, IMutable<bool> {}

sealed class RequiresUpdate : IRequiresUpdate
{
	public static RequiresUpdate Default { get; } = new();

	RequiresUpdate() : this(EmptyOperation<Action>.Default, new Variable<bool>()) {}

	readonly IOperation<Action> _operation;
	readonly IMutable<bool>     _store;

	public RequiresUpdate(IOperation<Action> operation, IMutable<bool> store)
	{
		_operation  = operation;
		_store = store;
	}

	public ValueTask Get(Action parameter) => _operation.Get(parameter);

	public bool Get() => _store.Get();

	public void Execute(bool parameter)
	{
		_store.Execute(parameter);
	}
}

sealed class RefreshActiveContent<T> : IRequiresUpdate
{
	readonly IResulting<T>  _result;
	readonly IMutable<bool> _state;
	readonly IMutable<int>  _counts;

	public RefreshActiveContent(IResulting<T> result, IMutable<int> counts)
		: this(result, new Variable<bool>(), counts) {}

	public RefreshActiveContent(IResulting<T> result, IMutable<bool> state, IMutable<int> counts)
	{
		_result     = result;
		_state = state;
		_counts     = counts;
	}

	public async ValueTask Get(Action parameter)
	{
		_counts.Execute(0);
		var result = _result.Get();
		await result;
		_state.Execute(true);
		parameter();
	}

	public bool Get() => _state.Get();

	public void Execute(bool parameter)
	{
		_state.Execute(parameter);
	}
}
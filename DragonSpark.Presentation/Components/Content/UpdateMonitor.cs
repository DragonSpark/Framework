using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

/*
sealed class UpdateMonitor : IUpdateMonitor
{
	public static UpdateMonitor Default { get; } = new();

	UpdateMonitor() : this(EmptyOperation<Action>.Default, new Switch()) {}

	readonly IOperation<Action> _operation;
	readonly IMutable<bool>     _store;

	public UpdateMonitor(IOperation<Action> operation, IMutable<bool> store)
	{
		_operation = operation;
		_store     = store;
	}

	public ValueTask Get(Action parameter) => _operation.Get(parameter);

	public bool Get() => _store.Get();

	public void Execute(bool parameter)
	{
		_store.Execute(parameter);
	}
}
*/

sealed class UpdateMonitor<T> : ICondition, IOperation<Action>
{
	readonly IResulting<T>  _result;
	readonly IMutable<bool> _state;
	readonly IMutable<int>  _counts;

	public UpdateMonitor(IResulting<T> result, IMutable<int> counts) : this(result, new Switch(), counts) {}

	public UpdateMonitor(IResulting<T> result, IMutable<bool> state, IMutable<int> counts)
	{
		_result = result;
		_state  = state;
		_counts = counts;
	}

	public async ValueTask Get(Action parameter)
	{
		_counts.Execute(0);
		await _result.Get();
		_state.Execute(true);
		parameter();
	}

	public bool Get(None _) => _state.Down();
}
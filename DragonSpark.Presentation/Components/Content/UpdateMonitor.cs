using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class UpdateMonitor : IUpdateMonitor
{
	public static UpdateMonitor Default { get; } = new();

	UpdateMonitor() : this(EmptyOperation<Action>.Default, new Variable<bool>()) {}

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
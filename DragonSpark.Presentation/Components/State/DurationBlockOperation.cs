using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Presentation.Components.State;

sealed class DurationBlockOperation : IOperation
{
	readonly IOperation _operation;
	readonly IDepending _allowed;

	public DurationBlockOperation(IOperation operation) : this(operation, TimeSpan.FromSeconds(1)) {}

	public DurationBlockOperation(IOperation operation, TimeSpan duration) : this(operation, new Blocker(duration)) {}

	public DurationBlockOperation(IOperation operation, IDepending allowed)
	{
		_operation = operation;
		_allowed   = allowed;
	}

	public async ValueTask Get()
	{
		if (await _allowed.On())
		{
			await _operation.Off();
		}
	}
}

sealed class DurationBlockOperation<T> : IOperation<T>
{
	readonly IOperation<T> _operation;
	readonly IDepending    _allowed;

	public DurationBlockOperation(IOperation<T> operation) : this(operation, TimeSpan.FromSeconds(1)) {}

	public DurationBlockOperation(IOperation<T> operation, TimeSpan duration)
		: this(operation, new Blocker(duration)) {}

	public DurationBlockOperation(IOperation<T> operation, IDepending allowed)
	{
		_operation = operation;
		_allowed   = allowed;
	}

	public async ValueTask Get(T parameter)
	{
		if (await _allowed.On())
		{
			await _operation.Off(parameter);
		}
	}
}
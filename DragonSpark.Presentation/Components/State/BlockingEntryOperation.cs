using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Presentation.Components.State;

sealed class BlockingEntryOperation : IOperation
{
	readonly IOperation _operation;
	readonly IDepending _allowed;

	public BlockingEntryOperation(IOperation operation) : this(operation, TimeSpan.FromSeconds(1)) {}

	public BlockingEntryOperation(IOperation operation, TimeSpan duration) : this(operation, new Blocker(duration)) {}

	public BlockingEntryOperation(IOperation operation, IDepending allowed)
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

sealed class BlockingEntryOperation<T> : IOperation<T>
{
	readonly IOperation<T> _operation;
	readonly IDepending    _allowed;

	public BlockingEntryOperation(IOperation<T> operation) : this(operation, TimeSpan.FromSeconds(1)) {}

	public BlockingEntryOperation(IOperation<T> operation, TimeSpan duration)
		: this(operation, new Blocker(duration)) {}

	public BlockingEntryOperation(IOperation<T> operation, IDepending allowed)
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
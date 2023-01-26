using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime.Execution;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class BlockingEntryOperation : IOperation
{
	readonly IOperation _operation;
	readonly ThreadAwareFirst      _active;
	readonly TimeSpan   _duration;

	public BlockingEntryOperation(IOperation operation) : this(operation, TimeSpan.FromSeconds(1)) {}

	public BlockingEntryOperation(IOperation operation, TimeSpan duration) : this(operation, new (), duration) {}

	public BlockingEntryOperation(IOperation operation, ThreadAwareFirst active, TimeSpan duration)
	{
		_operation = operation;
		_active    = active;
		_duration  = duration;
	}

	public async ValueTask Get()
	{
		if (_active.Get())
		{
			try
			{
				var captured = DateTimeOffset.Now;
				await _operation.Get();

				var elapsed = DateTimeOffset.Now - captured;
				if (elapsed < _duration)
				{
					await Task.Delay(_duration - elapsed);
				}
			}
			finally
			{
				_active.Execute();
			}
		}
	}
}

sealed class BlockingEntryOperation<T> : IOperation<T>
{
	readonly IOperation<T>  _operation;
	readonly ThreadAwareFirst _active;
	readonly TimeSpan       _duration;

	public BlockingEntryOperation(IOperation<T> operation) : this(operation, TimeSpan.FromSeconds(1)) {}

	public BlockingEntryOperation(IOperation<T> operation, TimeSpan duration) : this(operation, new (), duration) {}

	public BlockingEntryOperation(IOperation<T> operation, ThreadAwareFirst active, TimeSpan duration)
	{
		_operation = operation;
		_active    = active;
		_duration  = duration;
	}

	public async ValueTask Get(T parameter)
	{
		if (_active.Get())
		{
			try
			{
				var captured = DateTimeOffset.Now;
				await _operation.Get(parameter);

				var elapsed = DateTimeOffset.Now - captured;
				if (elapsed < _duration)
				{
					await Task.Delay(_duration - elapsed);
				}
			}
			finally
			{
				_active.Execute();
			}
		}
	}
}
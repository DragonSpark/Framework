using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	sealed class ThrottleEntryOperation : IOperation
	{
		readonly IOperation     _operation;
		readonly IMutable<bool> _active;
		readonly TimeSpan       _duration;

		public ThrottleEntryOperation(IOperation operation, TimeSpan duration)
			: this(operation, new Variable<bool>(), duration) {}

		public ThrottleEntryOperation(IOperation operation, IMutable<bool> active, TimeSpan duration)
		{
			_operation = operation;
			_active    = active;
			_duration  = duration;
		}

		public async ValueTask Get()
		{
			if (!_active.Get())
			{
				_active.Execute(true);
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
					_active.Execute(false);
				}
			}
		}
	}

	sealed class ThrottleEntryOperation<T> : IOperation<T>
	{
		readonly IOperation<T>  _operation;
		readonly IMutable<bool> _active;
		readonly TimeSpan       _duration;

		public ThrottleEntryOperation(IOperation<T> operation, TimeSpan duration)
			: this(operation, new Variable<bool>(), duration) {}

		public ThrottleEntryOperation(IOperation<T> operation, IMutable<bool> active, TimeSpan duration)
		{
			_operation = operation;
			_active    = active;
			_duration  = duration;
		}

		public async ValueTask Get(T parameter)
		{
			if (!_active.Get())
			{
				_active.Execute(true);
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
					_active.Execute(false);
				}
			}
		}
	}

}
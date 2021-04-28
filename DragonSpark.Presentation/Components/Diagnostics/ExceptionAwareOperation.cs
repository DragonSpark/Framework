using DragonSpark.Application.Diagnostics;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Diagnostics
{
	sealed class ExceptionAwareOperation : IOperation
	{
		readonly Type        _owner;
		readonly IExceptions _exceptions;
		readonly Func<Task>  _callback;

		public ExceptionAwareOperation(Type owner, IExceptions exceptions, Func<Task> callback)
		{
			_owner      = owner;
			_exceptions = exceptions;
			_callback   = callback;
		}

		public async ValueTask Get()
		{
			try
			{
				await _callback();
			}
			// ReSharper disable once CatchAllClause
			catch (Exception e)
			{
				await _exceptions.Get((_owner, e));
			}
		}
	}

	sealed class ExceptionAwareOperation<T> : IOperation<T>
	{
		readonly Type          _owner;
		readonly IExceptions   _exceptions;
		readonly Func<T, Task> _callback;

		public ExceptionAwareOperation(Type owner, IExceptions exceptions, Func<T, Task> callback)
		{
			_owner      = owner;
			_exceptions = exceptions;
			_callback   = callback;
		}

		public async ValueTask Get(T parameter)
		{
			try
			{
				await _callback(parameter);
			}
			// ReSharper disable once CatchAllClause
			catch (Exception e)
			{
				await _exceptions.Get((_owner, e));
			}
		}
	}

}

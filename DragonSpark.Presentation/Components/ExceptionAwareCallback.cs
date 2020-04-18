using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	// TODO: remove?
	sealed class ExceptionAwareCallback<T> : IOperation<T>
	{
		readonly Type        _owner;
		readonly IExceptions _exceptions;
		readonly Callback<T> _callback;

		public ExceptionAwareCallback(Type owner, IExceptions exceptions, Callback<T> callback)
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
using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State;

sealed class CancelAwareOperation : IOperation
{
	readonly IOperation   _previous;
	readonly ITokenHandle _handle;
	readonly IOperation?  _canceled;

	public CancelAwareOperation(IOperation previous, CancelAwareActivityOptions options)
		: this(previous, options.Handle, options.Canceled) {}

	public CancelAwareOperation(IOperation previous, ITokenHandle handle, IOperation? canceled)
	{
		_previous = previous;
		_handle   = handle;
		_canceled = canceled;
	}

	public async ValueTask Get()
	{
		var token = _handle.Token;
		try
		{
			await _previous.On();
		}
		catch (OperationCanceledException)
		{
			if (_canceled is not null)
			{
				await _canceled.Off();
			}

			if (!token.IsCancellationRequested)
			{
				await _handle.Off();
			}

			throw;
		}
	}
}

sealed class CancelAwareOperation<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly ITokenHandle  _handle;
	readonly IOperation?   _canceled;

	public CancelAwareOperation(IOperation<T> previous, CancelAwareActivityOptions options)
		: this(previous, options.Handle, options.Canceled) {}

	public CancelAwareOperation(IOperation<T> previous, ITokenHandle handle, IOperation? canceled)
	{
		_previous = previous;
		_handle   = handle;
		_canceled = canceled;
	}

	public async ValueTask Get(T parameter)
	{
		var token = _handle.Token;
		try
		{
			await _previous.On(parameter);
		}
		catch (OperationCanceledException)
		{
			if (_canceled is not null)
			{
				await _canceled.Off();
			}

			if (!token.IsCancellationRequested)
			{
				await _handle.Off();
			}

			throw;
		}
	}
}
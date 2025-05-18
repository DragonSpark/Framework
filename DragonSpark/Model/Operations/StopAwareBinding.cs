using DragonSpark.Compose;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

sealed class StopAwareBinding<T> : IOperation<CancellationToken>
{
	readonly IOperation<T>                         _operation;
	readonly Func<CancellationToken, ValueTask<T>> _input;
	readonly bool                                  _capture;

	public StopAwareBinding(IOperation<T> operation, Func<CancellationToken, ValueTask<T>> input, bool capture = false)
	{
		_operation = operation;
		_input     = input;
		_capture   = capture;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		var input    = _input(parameter);
		var instance = input.IsCompletedSuccessfully ? input.Result : await input.ConfigureAwait(_capture);
		await _operation.Off(instance);
	}
}
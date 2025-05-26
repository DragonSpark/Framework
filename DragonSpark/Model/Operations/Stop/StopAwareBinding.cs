using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

sealed class StopAwareBinding<T> : IOperation<CancellationToken>
{
	readonly ISelect<Stop<T>, ValueTask>           _previous;
	readonly Func<CancellationToken, ValueTask<T>> _input;
	readonly bool                                  _capture;

	public StopAwareBinding(ISelect<Stop<T>, ValueTask> previous, T instance) : this(previous, () => instance) {}

	public StopAwareBinding(ISelect<Stop<T>, ValueTask> previous, Func<T> input)
		: this(previous, _ => input().ToOperation()) {}

	public StopAwareBinding(ISelect<Stop<T>, ValueTask> previous, Func<CancellationToken, ValueTask<T>> input,
	                        bool capture = false)
	{
		_previous = previous;
		_input    = input;
		_capture  = capture;
	}

	public async ValueTask Get(CancellationToken parameter)
	{
		var input    = _input(parameter);
		var instance = input.IsCompletedSuccessfully ? input.Result : await input.ConfigureAwait(_capture);
		await _previous.Off(new(instance, parameter));
	}
}
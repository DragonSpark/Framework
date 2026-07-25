using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class Evaluate : IEvaluate
{
	readonly IJSRuntime _runtime;

	public Evaluate(IJSRuntime runtime) => _runtime = runtime;

	public ValueTask Get(Stop<string> parameter)
	{
		var (subject, stop) = parameter;
		return !subject.IsNullOrWhiteSpace()
			       ? _runtime.InvokeVoidAsync("eval", stop, subject)
			       : ValueTask.CompletedTask;
	}
}
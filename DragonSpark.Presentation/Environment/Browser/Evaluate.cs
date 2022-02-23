using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class Evaluate : IEvaluate
{
	readonly IJSRuntime _runtime;

	public Evaluate(IJSRuntime runtime) => _runtime = runtime;

	public ValueTask Get(string parameter)
		=> !string.IsNullOrWhiteSpace(parameter)
			   ? _runtime.InvokeVoidAsync("eval", parameter)
			   : ValueTask.CompletedTask;
}
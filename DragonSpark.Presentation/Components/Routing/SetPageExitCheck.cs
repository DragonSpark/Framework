using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Routing;

sealed class SetPageExitCheck : ISetPageExitCheck
{
	readonly IJSRuntime _runtime;

	public SetPageExitCheck(IJSRuntime runtime) => _runtime = runtime;

	public ValueTask Get(bool parameter) => _runtime.InvokeVoidAsync("cec_setEditorExitCheck", parameter);
}
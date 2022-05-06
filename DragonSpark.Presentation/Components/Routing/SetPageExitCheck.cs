using DragonSpark.Presentation.Environment.Browser;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Components.Routing;

sealed class SetPageExitCheck : BrowserCommand<bool>, ISetPageExitCheck
{
	public SetPageExitCheck(IJSRuntime runtime) : base(runtime, "cec_setEditorExitCheck") {}
}
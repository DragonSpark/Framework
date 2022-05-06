using DragonSpark.Presentation.Environment.Browser;
using Microsoft.JSInterop;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ScrollToFirstValidationMessage : BrowserCommand
{
	public ScrollToFirstValidationMessage(IJSRuntime runtime) : base(runtime, "scrollToFirstValidationMessage") {}
}
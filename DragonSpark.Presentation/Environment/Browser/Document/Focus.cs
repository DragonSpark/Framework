using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class Focus : IFocus
{
	readonly IFocusHandler _previous;

	public Focus(IFocusHandler previous) => _previous = previous;

	public ValueTask Get(Stop<ElementReference> parameter)
	{
		var (subject, stop) = parameter;
		stop.ThrowIfCancellationRequested();
		return _previous.FocusElementAsync(subject).ToOperation();
	}
}
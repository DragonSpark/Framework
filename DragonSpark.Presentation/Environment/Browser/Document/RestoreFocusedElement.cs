using DragonSpark.Compose;
using DragonSpark.Diagnostics;
using DragonSpark.Model.Operations;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class RestoreFocusedElement : IOperation
{
	readonly IFocusHandler _focus;

	public RestoreFocusedElement(IFocusHandler focus) => _focus = focus;

	public ValueTask Get() => _focus.RestoreStoredElementFocusAsync().ToOperation();
}

// TODO

sealed class ConnectionAwareRestoreFocusedElement : ConnectionAware
{
	public ConnectionAwareRestoreFocusedElement(RestoreFocusedElement previous) : base(previous) {}
}

sealed class PolicyAwareRestoreFocusedElement : PolicyAwareOperation
{
	public PolicyAwareRestoreFocusedElement(ConnectionAwareRestoreFocusedElement previous)
		: base(previous, ConnectionAwarePolicy.Default) {}
}
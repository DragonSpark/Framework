using DragonSpark.Model.Operations;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class FocusedElement : IFocusedElement
{
	readonly IFocusHandler _previous;

	public FocusedElement(IFocusHandler previous, ConnectionAwareStoreFocusedElement store,
	                      RestoreFocusedElement restore)
	{
		_previous = previous;
		Store     = store;
		Restore   = restore;
	}

	public IOperation Restore { get; }

	public IOperation Store { get; }

	public ValueTask DisposeAsync() => _previous.DisposeAsync();
}
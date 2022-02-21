using DragonSpark.Model.Operations;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

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

	public async ValueTask DisposeAsync()
	{
		try
		{
			await _previous.DisposeAsync().ConfigureAwait(false);
		}
		catch (JSDisconnectedException) {}
	}
}
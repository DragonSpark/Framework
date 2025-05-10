using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;

namespace DragonSpark.Presentation.Environment.Browser.Document;

sealed class FocusedElement : Disposing, IFocusedElement
{
	public FocusedElement(IFocusHandler previous, PolicyAwareFocusedElement store,
	                      PolicyAwareRestoreFocusedElement restore)
		: base(previous)
	{
		Store   = store;
		Restore = restore;
	}

	public IOperation Store { get; }

	public IOperation Restore { get; }
}
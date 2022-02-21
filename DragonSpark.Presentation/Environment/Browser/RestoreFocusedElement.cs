using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class RestoreFocusedElement : IOperation
{
	readonly IFocusHandler _focus;

	public RestoreFocusedElement(IFocusHandler focus) => _focus = focus;

	public ValueTask Get() => _focus.RestoreStoredElementFocusAsync().ToOperation();
}
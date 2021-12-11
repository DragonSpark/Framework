using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Majorsoft.Blazor.Components.Common.JsInterop.Focus;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Browser;

sealed class StoreFocusedElement : IOperation
{
	readonly IFocusHandler _focus;

	public StoreFocusedElement(IFocusHandler focus) => _focus = focus;

	public ValueTask Get() => _focus.StoreFocusedElementAsync().ToOperation();
}
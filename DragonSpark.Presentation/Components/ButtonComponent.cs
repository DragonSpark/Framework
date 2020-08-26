using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace DragonSpark.Presentation.Components
{
	public class ButtonComponent : RadzenButton
	{
		[Parameter]
		public string? CssClass { get; set; }

		protected override string GetComponentCssClass() => CssClass.NullIfEmpty() ?? base.GetComponentCssClass();
	}
}
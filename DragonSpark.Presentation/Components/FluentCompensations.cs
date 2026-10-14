using Microsoft.FluentUI.AspNetCore.Components;

namespace DragonSpark.Presentation.Components;

public static class FluentCompensations
{
	public static string? GetHiddenAttribute(GridItemHidden? parameter)
		=> Components.GetHiddenAttribute.Default.Get(parameter);
}
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Syncfusion.Components;

public class GridFilterSettings : global::Syncfusion.Blazor.Grids.GridFilterSettings
{
	[Parameter]
	public FilterType FilterType
	{
		get => (FilterType)Type;
		set => Type = (global::Syncfusion.Blazor.Grids.FilterType)value;
	}
}
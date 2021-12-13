using Microsoft.AspNetCore.Components;

namespace DragonSpark.Syncfusion.Components;

public class GridColumnDefinition : global::Syncfusion.Blazor.Grids.GridColumn
{
	[Parameter]
	public ClipMode Clipping
	{
		get => (ClipMode)ClipMode;
		set => ClipMode = (global::Syncfusion.Blazor.Grids.ClipMode)value;
	}
}
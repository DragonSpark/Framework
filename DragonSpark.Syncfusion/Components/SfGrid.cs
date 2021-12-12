using Microsoft.AspNetCore.Components;

namespace DragonSpark.Syncfusion.Components;

public class SfGrid<TValue> : global::Syncfusion.Blazor.Grids.SfGrid<TValue>
{
	[Parameter]
	public RowDirection RowDirection
	{
		get => (RowDirection)RowRenderingMode;
		set => RowRenderingMode = (global::Syncfusion.Blazor.Grids.RowDirection)value;
	}

	[Parameter]
	public ClipMode Clipping
	{
		get => (ClipMode)ClipMode;
		set => ClipMode = (global::Syncfusion.Blazor.Grids.ClipMode)value;
	}
}

public enum ClipMode
{
	Clip,
	Ellipsis,
	EllipsisWithTooltip,
}
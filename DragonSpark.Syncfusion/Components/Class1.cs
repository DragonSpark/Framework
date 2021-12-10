using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;
using System.Collections.Generic;

namespace DragonSpark.Syncfusion.Components;

internal class Class1 {}

public class SfGrid<TValue> : global::Syncfusion.Blazor.Grids.SfGrid<TValue>
{
	[Parameter]
	public RowDirection RowDirection
	{
		get => (RowDirection)RowRenderingMode;
		set => RowRenderingMode = (global::Syncfusion.Blazor.Grids.RowDirection)value;
	}
}

public sealed class DefaultToolbar : List<string>
{
	public static DefaultToolbar Default { get; } = new();

	DefaultToolbar() : base(new[] { "Filter" }) {}
}

public class SfDataManager : global::Syncfusion.Blazor.Data.SfDataManager
{
	public SfDataManager() => Adaptor = Adaptors.CustomAdaptor;
}

public class GridPageSettings : global::Syncfusion.Blazor.Grids.GridPageSettings {}

public class GridFilterSettings : global::Syncfusion.Blazor.Grids.GridFilterSettings
{
	[Parameter]
	public FilterType FilterType
	{
		get => (FilterType)Type;
		set => Type = (global::Syncfusion.Blazor.Grids.FilterType)value;
	}
}

public class GridColumns : global::Syncfusion.Blazor.Grids.GridColumns {}

public enum FilterType
{
	FilterBar,
	Excel,
	Menu,
	CheckBox,
}

public enum RowDirection
{
	Horizontal,
	Vertical,
}
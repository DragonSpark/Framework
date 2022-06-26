using DragonSpark.Model.Results;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Grids;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class DateTimeFilterSettings : Instance<FilterSettings>
{
	public static DateTimeFilterSettings Default { get; } = new();

	DateTimeFilterSettings()
		: base(new FilterSettings { Operator = Operator.GreaterThanOrEqual, Type = FilterType.Menu }) {}
}
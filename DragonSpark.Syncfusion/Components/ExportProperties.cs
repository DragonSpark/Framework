using DragonSpark.Model.Results;
using Syncfusion.Blazor.Grids;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class ExportProperties : Instance<ExcelExportProperties>
{
	public static ExportProperties Default { get; } = new();

	ExportProperties() : base(new() { IncludeTemplateColumn = true }) {}
}
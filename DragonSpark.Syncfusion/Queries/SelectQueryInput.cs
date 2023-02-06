using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class SelectQueryInput : ISelect<DataManagerRequest, SyncfusionPageInput>
{
	public static SelectQueryInput Default { get; } = new();

	SelectQueryInput() {}

	public SyncfusionPageInput Get(DataManagerRequest parameter) => new(parameter);
}
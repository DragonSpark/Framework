using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class SelectQueryInput : ISelect<Stop<DataManagerRequest>, SyncfusionPageInput>
{
	public static SelectQueryInput Default { get; } = new();

	SelectQueryInput() {}

	public SyncfusionPageInput Get(Stop<DataManagerRequest> parameter) => new(parameter, parameter);
}
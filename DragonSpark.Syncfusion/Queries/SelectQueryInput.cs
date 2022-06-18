using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class SelectQueryInput : ISelect<DataManagerRequest, SyncfusionPageInput>
{
	public static SelectQueryInput Default { get; } = new SelectQueryInput();

	SelectQueryInput() {}

	public SyncfusionPageInput Get(DataManagerRequest parameter)
		=> new()
		{
			Request = parameter,
			Partition = parameter.Skip > 0 || parameter.Take > 0
				            ? new Partition(parameter.Skip > 0 ? parameter.Skip : null,
				                            parameter.Take > 0 ? parameter.Take : null)
				            : null,
			IncludeTotalCount = parameter.RequiresCounts
		};
}
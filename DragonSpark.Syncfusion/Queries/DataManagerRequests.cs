using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class DataManagerRequests : ISelect<PageInput, DataManagerRequest>
{
	public static DataManagerRequests Default { get; } = new();

	DataManagerRequests() {}

	public DataManagerRequest Get(PageInput parameter)
		=> parameter is SyncfusionPageInput input
			   ? input.Request
			   : throw new InvalidOperationException("SyncfusionPageInput required");
}
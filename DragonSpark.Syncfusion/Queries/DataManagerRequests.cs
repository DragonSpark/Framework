using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Model.Selection.Stores;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class DataManagerRequests : ReferenceValueTable<PageInput, DataManagerRequest>
{
	public static DataManagerRequests Default { get; } = new();

	DataManagerRequests() {}
}
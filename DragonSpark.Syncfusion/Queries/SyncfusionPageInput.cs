using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using Syncfusion.Blazor;
using System.Text.Json.Serialization;

namespace DragonSpark.SyncfusionRendering.Queries;

[method: JsonConstructor]
public sealed record SyncfusionPageInput(DataManagerRequest Request, bool IncludeTotalCount, Partition? Partition)
	: PageInput(IncludeTotalCount, null, null, Partition)
{
	public SyncfusionPageInput(DataManagerRequest Request)
		: this(Request, Request.RequiresCounts, Request.Partition()) {}
}
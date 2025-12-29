using DragonSpark.Contracts.Queries;
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
using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class SyncfusionPageInput : PageInput
{
	public DataManagerRequest Request { get; set; } = default!;
}
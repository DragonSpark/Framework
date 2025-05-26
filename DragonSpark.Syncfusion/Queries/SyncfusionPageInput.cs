using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using Syncfusion.Blazor;
using System.Threading;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed record SyncfusionPageInput(DataManagerRequest Request, CancellationToken Stop)
	: PageInput(Request.RequiresCounts, null, null, Request.Skip > 0 || Request.Take > 0
		                                                ? new(Request.Skip > 0 ? Request.Skip : null,
		                                                      Request.Take > 0 ? Request.Take : null)
		                                                : null, Stop);
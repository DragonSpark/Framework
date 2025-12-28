using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using PageInput = DragonSpark.Contracts.Queries.PageInput;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

public class PagedQuery<TIn, TOut> : PagedQueryBase<TIn, TOut>
{
	protected PagedQuery(IScopes scopes, IQuery<TIn, TOut> query, IPaging<TOut> paging)
		: base(new RuntimeQuery(scopes, query),
		       DataManagerRequests.Default.Then().Accept<PageInput>(x => (PageRequest)x).Get(),
		       paging) {}
}
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;
using PageInput = DragonSpark.Contracts.Queries.PageInput;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

public class PagedQuery<TIn, TOut> : PagedQueryBase<TIn, TOut>
{
	protected PagedQuery(IScopes scopes, IPaging<TOut> paging, IQuery<TIn, TOut> query)
		: base(new RuntimeQuery(scopes, query),
		       DataManagerRequests.Default.Then().Accept<PageInput>(x => (PageRequest)x).Get(),
		       paging) {}
}

public class PagedQuery<T> : PagedQueryBase<None, T>, IPagedQuery<T>
{
	protected PagedQuery(IRuntimeQuery<None, T> query, ISelect<PageInput, DataManagerRequest> select, IPaging<T> paging)
		: base(query, select, paging) {}

	protected PagedQuery(IRuntimeQuery<None, T> query, ICompose<T> compose, IPaging<T> paging) 
		: base(query, compose, paging) {}
}
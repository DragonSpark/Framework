using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

namespace DragonSpark.SyncfusionRendering.Queries;

public class PagedQuery<TIn, TOut> 
	: DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination.PagedQuery<TIn, TOut>
{
	protected PagedQuery(IScopes scopes, IQuery<TIn, TOut> query, IPaging<TOut> paging)
		: this(new RuntimeQuery(scopes, query), paging) {}

	protected PagedQuery(IRuntimeQuery<TIn, TOut> query, IPaging<TOut> paging)
		: base(query, SyncfusionCompose<TOut>.Default, paging) {}

	sealed class RuntimeQuery : StandardRuntimeQuery<TIn, TOut>
	{
		public RuntimeQuery(IScopes scopes, IQuery<TIn, TOut> query) : base(scopes, query) {}
	}
}
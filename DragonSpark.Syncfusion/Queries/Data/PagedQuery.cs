using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;
using PageInput = DragonSpark.Contracts.Queries.PageInput;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

public abstract class PagedQuery<TIn, TOut> : IPagedQuery<TIn, TOut>
{
	readonly DragonSpark.SyncfusionRendering.Queries.PagedQuery<TIn, TOut> _previous;

	protected PagedQuery(IScopes scopes, IQuery<TIn, TOut> query, IPaging<TOut> paging)
		: this(new Page(scopes, query, paging)) {}

	protected PagedQuery(DragonSpark.SyncfusionRendering.Queries.PagedQuery<TIn, TOut> previous)
		=> _previous = previous;

	sealed class Page : DragonSpark.SyncfusionRendering.Queries.PagedQuery<TIn, TOut>
	{
		public Page(IScopes scopes, IQuery<TIn, TOut> query, IPaging<TOut> paging)
			: base(new RuntimeQuery(scopes, query),
			       DataManagerRequests.Default.Then().Accept<PageInput>(x => (PageRequest)x).Get(),
			       paging) {}
	}

	public async ValueTask<PageResult<TOut>> Get(Stop<PageQueryInput<TIn>> parameter)
	{
		var previous = await _previous.Off(parameter);
		return new(previous, previous.Total);
	}
}
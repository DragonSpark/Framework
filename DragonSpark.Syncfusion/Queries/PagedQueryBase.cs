using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;

namespace DragonSpark.SyncfusionRendering.Queries;

public class PagedQueryBase<TIn, TOut> : Application.AspNet.Entities.Queries.Runtime.Pagination.PagedQueryBase<TIn, TOut>
{
	protected PagedQueryBase(IRuntimeQuery<TIn, TOut> query, ISelect<PageInput, DataManagerRequest> select,
	                         IPaging<TOut> paging)
		: this(query, new SyncfusionCompose<TOut>(select), paging) {}

	protected PagedQueryBase(IRuntimeQuery<TIn, TOut> query, ICompose<TOut> compose, IPaging<TOut> paging)
		: base(query, compose, paging) {}

	protected sealed class RuntimeQuery : StandardRuntimeQuery<TIn, TOut>
	{
		public RuntimeQuery(IScopes scopes, IQuery<TIn, TOut> query) : base(scopes, query) {}
	}
}
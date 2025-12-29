using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public class PagedQueryBase<TIn, TOut> : IPagedQuery<TIn, TOut>
{
	readonly IRuntimeQuery<TIn, TOut> _query;
	readonly ICompose<TOut>           _compose;
	readonly IPaging<TOut>            _paging;

	protected PagedQueryBase(IRuntimeQuery<TIn, TOut> query, ICompose<TOut> compose, IPaging<TOut> paging)
	{
		_query   = query;
		_compose = compose;
		_paging  = paging;
	}

	public async ValueTask<PageResult<TOut>> Get(Stop<PageQueryInput<TIn>> parameter)
	{
		var ((subject, input), stop) = parameter;
		var queries = _query.Get(subject);
		var result  = await _paging.Get(new(queries, _compose)).Off(new(input, stop));
		return result;
	}
}
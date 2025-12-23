using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

public interface IPaging<T> : ISelect<PagingInput<T>, IPages<T>>;

// TODO

public readonly record struct PageQueryInput<T>(T Parameter, PageInput Input);

public class PagedQuery<TIn, TOut> : IStopAware<PageQueryInput<TIn>, Page<TOut>>
{
	readonly IRuntimeQuery<TIn, TOut> _query;
	readonly ICompose<TOut>           _compose;
	readonly IPaging<TOut>            _paging;

	protected PagedQuery(IRuntimeQuery<TIn, TOut> query, ICompose<TOut> compose, IPaging<TOut> paging)
	{
		_query   = query;
		_compose = compose;
		_paging  = paging;
	}

	public async ValueTask<Page<TOut>> Get(Stop<PageQueryInput<TIn>> parameter)
	{
		var ((subject, input), stop) = parameter;
		var queries      = _query.Get(subject);
		var result       = await _paging.Get(new(queries, _compose)).Off(new(input, stop));
		return result;
	}
}

/**/
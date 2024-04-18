using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class QueriedPages<T> : IPages<T>
{
	readonly IQueries<T> _queries;
	readonly ICompose<T> _compose;
	readonly IToArray<T> _materialize;

	public QueriedPages(IQueries<T> queries, ICompose<T> compose) : this(queries, compose, DefaultToArray<T>.Default) {}

	public QueriedPages(IQueries<T> queries, ICompose<T> compose, IToArray<T> materialize)
	{
		_queries     = queries;
		_compose     = compose;
		_materialize = materialize;
	}

	public async ValueTask<Page<T>> Get(PageInput parameter)
	{
		using var session = _queries.Get();
		var (query, count) = await _compose.Await(new(parameter, session.Subject));
		var materialize = await _materialize.Await(new(query, parameter.Token));
		return new(materialize.Open(), count);
	}
}
using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

sealed class Pages<T> : IPages<T>
{
	readonly IQueries<T> _queries;
	readonly ICompose<T> _compose;
	readonly IToArray<T> _materialize;

	public Pages(IQueries<T> queries, ICompose<T> compose) : this(queries, compose, DefaultToArray<T>.Default) {}

	public Pages(IQueries<T> queries, ICompose<T> compose, IToArray<T> materialize)
	{
		_queries     = queries;
		_compose     = compose;
		_materialize = materialize;
	}

	public async ValueTask<Page<T>> Get(PageInput parameter)
	{
		using var session = await _queries.Await();
		var (query, count) = await _compose.Await(new(parameter, session.Subject));
		var materialize = await _materialize.Await(query);
		return new(materialize.Open(), count);
	}
}
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Materialize;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;

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

	public async ValueTask<Page<T>> Get(Stop<PageInput> parameter)
	{
		var (subject, stop) = parameter;
		using var session = await _queries.Off(stop);
		var (query, count) = await _compose.Off(new(new(subject, session.Subject), stop));
		var materialize = await _materialize.Off(new(query, stop));
		return new(materialize.Open(), count);
	}
}
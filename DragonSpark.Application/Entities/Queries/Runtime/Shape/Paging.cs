using DragonSpark.Application.Entities.Queries.Runtime.Materialize;
using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape
{
	sealed class Paging<T> : IPaging<T>
	{
		readonly IQueries<T> _queries;
		readonly ICompose<T> _compose;
		readonly IToArray<T> _materialize;

		public Paging(IQueries<T> queries, ICompose<T> compose) : this(queries, compose, DefaultToArray<T>.Default) {}

		public Paging(IQueries<T> queries, ICompose<T> compose, IToArray<T> materialize)
		{
			_queries     = queries;
			_compose     = compose;
			_materialize = materialize;
		}

		public async ValueTask<Current<T>> Get(QueryInput parameter)
		{
			using var session = await _queries.Await();
			var (query, count) = await _compose.Await(new(parameter, session.Subject));
			var materialize = await _materialize.Await(query);
			return new(materialize.Open(), count);
		}
	}
}
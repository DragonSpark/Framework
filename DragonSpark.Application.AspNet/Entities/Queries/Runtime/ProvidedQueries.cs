using DragonSpark.Compose;
using DragonSpark.Runtime;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public class ProvidedQueries<T> : IQueries<T>
{
	readonly Func<ValueTask<T[]>> _previous;

	public ProvidedQueries(Func<ValueTask<T[]>> previous) => _previous = previous;

	public async ValueTask<Query<T>> Get()
	{
		var previous = await _previous().Await();
		return new (previous.AsQueryable(), EmptyDisposable.Default);
	}
}
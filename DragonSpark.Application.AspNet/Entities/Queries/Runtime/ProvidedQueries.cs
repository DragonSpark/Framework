using System;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Runtime;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

public class ProvidedQueries<T> : IQueries<T>
{
	readonly Func<ValueTask<T[]>> _previous;

	public ProvidedQueries(Func<ValueTask<T[]>> previous) => _previous = previous;

	[MustDisposeResource]
	public async ValueTask<Query<T>> Get()
	{
		var previous = await _previous().Off();
		return new (previous.AsQueryable(), EmptyDisposable.Default);
	}
}

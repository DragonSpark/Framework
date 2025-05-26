using DragonSpark.Compose;
using DragonSpark.Runtime;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

public class ProvidedQueries<T> : IQueries<T>
{
	readonly Func<CancellationToken, ValueTask<T[]>> _previous;

	protected ProvidedQueries(Func<CancellationToken, ValueTask<T[]>> previous) => _previous = previous;

	[MustDisposeResource]
	public async ValueTask<Query<T>> Get(CancellationToken parameter)
	{
		var previous = await _previous(parameter).Off();
		return new (previous.AsQueryable(), EmptyDisposable.Default);
	}
}

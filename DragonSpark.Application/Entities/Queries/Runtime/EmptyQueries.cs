using DragonSpark.Model;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public sealed class EmptyQueries<T> : IQueries<T>
{
	public static EmptyQueries<T> Default { get; } = new();

	EmptyQueries() {}

	public Query<T> Get() => new(Empty.Queryable<T>(), EmptyDisposable.Default);
}
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime;

public sealed class EmptyQueries<T> : IQueries<T>
{
	public static EmptyQueries<T> Default { get; } = new();

	EmptyQueries() {}

	public ValueTask<Query<T>> Get() => new Query<T>(Empty.Queryable<T>(), EmptyDisposable.Default).ToOperation();
}
using System.Threading.Tasks;
using DragonSpark.Model;
using DragonSpark.Runtime;
using JetBrains.Annotations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime;

public sealed class EmptyQueries<T> : IQueries<T>
{
	public static EmptyQueries<T> Default { get; } = new();

	EmptyQueries() {}

	[MustDisposeResource]
	public ValueTask<Query<T>> Get() => new(new Query<T>(Empty.Queryable<T>(), EmptyDisposable.Default));
}

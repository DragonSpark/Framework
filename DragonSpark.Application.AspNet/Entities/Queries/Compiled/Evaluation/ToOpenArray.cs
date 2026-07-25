using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToOpenArray<T> : IEvaluate<T, T[]>
{
	public static ToOpenArray<T> Default { get; } = new ();

	ToOpenArray() {}

	public ValueTask<T[]> Get(Stop<IAsyncEnumerable<T>> parameter) => parameter.Subject.ToArrayAsync(parameter.Token);
}
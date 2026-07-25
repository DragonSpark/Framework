using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToFirstOrDefault<T> : IEvaluate<T, T?>
{
	public static ToFirstOrDefault<T> Default { get; } = new();

	ToFirstOrDefault() {}

	public ValueTask<T?> Get(Stop<IAsyncEnumerable<T>> parameter) => parameter.Subject.FirstOrDefaultAsync(parameter.Token);
}
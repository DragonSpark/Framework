using DragonSpark.Model.Operations;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToSum : IEvaluate<decimal, decimal>
{
	public static ToSum Default { get; } = new();

	ToSum() {}

	public ValueTask<decimal> Get(Stop<IAsyncEnumerable<decimal>> parameter) => parameter.Subject.SumAsync(parameter.Token);
}
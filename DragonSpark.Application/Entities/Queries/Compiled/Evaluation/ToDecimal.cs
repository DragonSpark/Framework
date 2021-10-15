using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToDecimal : IEvaluate<decimal, decimal>
{
	public static ToDecimal Default { get; } = new();

	ToDecimal() {}

	public ValueTask<decimal> Get(IAsyncEnumerable<decimal> parameter)
		=> parameter.AsAsyncValueEnumerable().SumAsync();
}
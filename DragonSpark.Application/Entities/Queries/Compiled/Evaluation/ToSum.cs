using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToSum : IEvaluate<decimal, decimal>
{
	public static ToSum Default { get; } = new();

	ToSum() {}

	public ValueTask<decimal> Get(IAsyncEnumerable<decimal> parameter) => parameter.SumAsync();
}
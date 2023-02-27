using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToAverage : IEvaluate<decimal, decimal>
{
	public static ToAverage Default { get; } = new();

	ToAverage() {}

	public ValueTask<decimal> Get(IAsyncEnumerable<decimal> parameter) => parameter.AverageAsync();
}
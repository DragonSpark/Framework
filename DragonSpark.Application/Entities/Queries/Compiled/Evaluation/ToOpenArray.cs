using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToOpenArray<T> : IEvaluate<T, T[]>
{
	public static ToOpenArray<T> Default { get; } = new ();

	ToOpenArray() {}

	public ValueTask<T[]> Get(IAsyncEnumerable<T> parameter) => parameter.ToArrayAsync();
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToList<T> : IEvaluate<T, List<T>>
{
	public static ToList<T> Default { get; } = new ToList<T>();

	ToList() {}

	public ValueTask<List<T>> Get(IAsyncEnumerable<T> parameter) => parameter.ToListAsync();
}
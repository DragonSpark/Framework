using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToArray<T> : IEvaluate<T, Array<T>>
{
	public static ToArray<T> Default { get; } = new ToArray<T>();

	ToArray() {}

	public async ValueTask<Array<T>> Get(IAsyncEnumerable<T> parameter) => await parameter.ToArrayAsync();
}
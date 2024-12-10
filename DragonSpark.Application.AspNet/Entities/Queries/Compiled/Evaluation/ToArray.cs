using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToArray<T> : IEvaluate<T, Array<T>>
{
	public static ToArray<T> Default { get; } = new();

	ToArray() {}

	public async ValueTask<Array<T>> Get(IAsyncEnumerable<T> parameter) => await parameter.ToArrayAsync().Await();
}

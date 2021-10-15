using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToFirst<T> : IEvaluate<T, T>
{
	public static ToFirst<T> Default { get; } = new ();

	ToFirst() {}

	public ValueTask<T> Get(IAsyncEnumerable<T> parameter) => parameter.FirstAsync();
}
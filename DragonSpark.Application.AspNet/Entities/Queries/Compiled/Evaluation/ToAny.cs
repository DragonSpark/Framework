using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToAny<T> : IEvaluate<T, bool>
{
	public static ToAny<T> Default { get; } = new();

	ToAny() {}

	public ValueTask<bool> Get(IAsyncEnumerable<T> parameter) => parameter.AnyAsync();
}
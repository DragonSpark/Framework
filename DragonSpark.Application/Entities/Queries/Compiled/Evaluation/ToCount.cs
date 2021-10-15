using DragonSpark.Compose;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToCount<T> : IEvaluate<T, uint>
{
	public static ToCount<T> Default { get; } = new();

	ToCount() {}

	public async ValueTask<uint> Get(IAsyncEnumerable<T> parameter)
	{
		var count  = await parameter.CountAsync();
		var result = count.Grade();
		return result;
	}
}
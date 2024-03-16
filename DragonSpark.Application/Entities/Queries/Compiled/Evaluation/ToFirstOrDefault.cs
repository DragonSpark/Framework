using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToFirstOrDefault<T> : IEvaluate<T, T?>
{
	public static ToFirstOrDefault<T> Default { get; } = new();

	ToFirstOrDefault() {}

	public async ValueTask<T?> Get(IAsyncEnumerable<T> parameter)
	{
		await foreach (var item in parameter.ConfigureAwait(false))
		{
			return item;
		}

		return default;
	}
}
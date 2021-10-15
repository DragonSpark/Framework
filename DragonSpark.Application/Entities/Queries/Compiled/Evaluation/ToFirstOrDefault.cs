using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToFirstOrDefault<T> : IEvaluate<T, T?>
{
	public static ToFirstOrDefault<T> Default { get; } = new ToFirstOrDefault<T>();

	ToFirstOrDefault() {}

	public ValueTask<T?> Get(IAsyncEnumerable<T> parameter) => parameter.FirstOrDefaultAsync();
}
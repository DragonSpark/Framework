using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	sealed class FirstOrDefault<T> : IEvaluate<T, T?>
	{
		public static FirstOrDefault<T> Default { get; } = new FirstOrDefault<T>();

		FirstOrDefault() {}

		public ValueTask<T?> Get(IAsyncEnumerable<T> parameter) => parameter.FirstOrDefaultAsync();
	}
}
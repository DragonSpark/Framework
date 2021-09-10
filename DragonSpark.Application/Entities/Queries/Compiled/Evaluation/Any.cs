using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	sealed class Any<T> : IEvaluate<T, bool>
	{
		public static Any<T> Default { get; } = new Any<T>();

		Any() {}

		public ValueTask<bool> Get(IAsyncEnumerable<T> parameter) => parameter.AnyAsync();
	}
}
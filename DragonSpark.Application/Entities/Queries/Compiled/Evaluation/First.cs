using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	sealed class First<T> : IEvaluate<T, T>
	{
		public static First<T> Default { get; } = new ();

		First() {}

		public ValueTask<T> Get(IAsyncEnumerable<T> parameter) => parameter.FirstAsync();
	}
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	sealed class SingleOrDefault<T> : IEvaluate<T, T?>
	{
		public static SingleOrDefault<T> Default { get; } = new ();

		SingleOrDefault() {}

		public ValueTask<T?> Get(IAsyncEnumerable<T> parameter) => parameter.SingleOrDefaultAsync();
	}
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Evaluation
{
	sealed class Single<T> : IEvaluate<T, T>
	{
		public static Single<T> Default { get; } = new Single<T>();

		Single() {}

		public ValueTask<T> Get(IAsyncEnumerable<T> parameter) => parameter.SingleAsync(); // ISSUE: https://github.com/NetFabric/NetFabric.Hyperlinq/issues/375
	}
}
using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToNumber : IEvaluate<int, uint>
{
	public static ToNumber Default { get; } = new();

	ToNumber() {}

	public async ValueTask<uint> Get(IAsyncEnumerable<int> parameter)
		=> (uint)await parameter.AsAsyncValueEnumerable().SumAsync().Await();
}

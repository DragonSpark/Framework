using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToNumber : IEvaluate<int, uint>
{
	public static ToNumber Default { get; } = new();

	ToNumber() {}

	public async ValueTask<uint> Get(IAsyncEnumerable<int> parameter)
		=> (uint)await parameter.AsAsyncValueEnumerable().SumAsync();
}
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using NetFabric.Hyperlinq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

sealed class ToNumber : IEvaluate<int, uint>
{
	public static ToNumber Default { get; } = new();

	ToNumber() {}

	public async ValueTask<uint> Get(Stop<IAsyncEnumerable<int>> parameter)
		=> (uint)await parameter.Subject.AsAsyncValueEnumerable().SumAsync(parameter.Token).Off();
}

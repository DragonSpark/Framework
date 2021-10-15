using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public sealed class ToLease<T> : IEvaluate<T, Leasing<T>>
{
	public static ToLease<T> Default { get; } = new ToLease<T>();

	ToLease() {}

	public async ValueTask<Leasing<T>> Get(IAsyncEnumerable<T> parameter)
	{
		var owner  = await parameter.AsAsyncValueEnumerable().ToArrayAsync(ArrayPool<T>.Shared);
		var result = owner.Then();
		return result;
	}
}
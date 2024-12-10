using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public sealed class ToLease<T> : IEvaluate<T, Leasing<T>>
{
	public static ToLease<T> Default { get; } = new();

	ToLease() {}

	[MustDisposeResource]
	public async ValueTask<Leasing<T>> Get(IAsyncEnumerable<T> parameter)
	{
		var owner  = await parameter.AsAsyncValueEnumerable().ToArrayAsync(ArrayPool<T>.Shared).Await();
		var result = owner.Then();
		return result;
	}
}

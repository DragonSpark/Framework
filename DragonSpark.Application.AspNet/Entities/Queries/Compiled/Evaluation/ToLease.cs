using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences.Memory;
using JetBrains.Annotations;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;

public sealed class ToLease<T> : IEvaluate<T, Leasing<T>>
{
	public static ToLease<T> Default { get; } = new();

	ToLease() {}

	[MustDisposeResource]
	public async ValueTask<Leasing<T>> Get(Stop<IAsyncEnumerable<T>> parameter)
	{
		var owner = await parameter.Subject.AsAsyncValueEnumerable()
		                           .ToArrayAsync(ArrayPool<T>.Shared, parameter.Token)
		                           .Off();
		var result = owner.Then();
		return result;
	}
}
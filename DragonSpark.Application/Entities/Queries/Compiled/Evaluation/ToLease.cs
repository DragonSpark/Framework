using DragonSpark.Compose;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation
{
	sealed class ToLease<T> : IEvaluate<T, DragonSpark.Model.Sequences.Memory.Lease<T>>
	{
		public static ToLease<T> Default { get; } = new ToLease<T>();

		ToLease() {}

		public async ValueTask<DragonSpark.Model.Sequences.Memory.Lease<T>> Get(IAsyncEnumerable<T> parameter)
		{
			var owner  = await parameter.AsAsyncValueEnumerable().ToArrayAsync(ArrayPool<T>.Shared);
			var result = owner.AsLease();
			return result;
		}
	}
}
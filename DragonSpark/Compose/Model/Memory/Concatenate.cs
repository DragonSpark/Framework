using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Runtime;
using NetFabric.Hyperlinq;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class Concatenate<T> : ILease<(Lease<T> First, EnumerableExtensions.ValueEnumerable<T> Second), T>
	{
		public static Concatenate<T> Default { get; } = new Concatenate<T>();

		Concatenate() {}

		public Lease<T> Get((Lease<T> First, EnumerableExtensions.ValueEnumerable<T> Second) parameter)
		{
			var (first, second) = parameter;

			using var builder = ArrayBuilder.New<T>(first.ActualLength * 2);
			builder.Add(first.AsMemory());

			foreach (var element in second)
			{
				builder.Add(element);
			}

			first.Dispose();
			return builder.AsLease();
		}
	}
}
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Runtime;
using NetFabric.Hyperlinq;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class Concatenate<T>
		: ISelect<(Lease<T> First, EnumerableExtensions.ValueEnumerable<T> Second), Concatenation<T>>
	{
		public static Concatenate<T> Default { get; } = new Concatenate<T>();

		Concatenate() {}

		public Concatenation<T> Get((Lease<T> First, EnumerableExtensions.ValueEnumerable<T> Second) parameter)
		{
			var (first, second) = parameter;

			using var builder = ArrayBuilder.New<T>(first.ActualLength * 2);
			builder.Add(first.AsMemory());

			foreach (var element in second)
			{
				builder.Add(element);
			}

			return new Concatenation<T>(first, builder.AsLease());
		}
	}
}
using DragonSpark.Model.Selection.Alterations;
using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class Distinct<T> : IAlteration<Lease<T>>
	{
		public static Distinct<T> Default { get; } = new Distinct<T>();

		Distinct() {}

		public Lease<T> Get(Lease<T> parameter)
		{
			var index       = 0;
			var destination = parameter.AsSpan();
			foreach (var element in destination.AsValueEnumerable().Distinct())
			{
				destination[index++] = element;
			}

			return parameter.Size(index);
		}
	}
}
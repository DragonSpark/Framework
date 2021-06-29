using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class Distinct<T> // : IAlteration<Lease<T>>
	{
		public static Distinct<T> Default { get; } = new Distinct<T>();

		Distinct() {}

		public Lease<T> Get(in Lease<T> parameter)
		{
			var index = 0u;
			foreach (var element in parameter.AsSpan().Distinct())
			{
				parameter[index++] = element;
			}

			return parameter.Size(index);
		}
	}
}
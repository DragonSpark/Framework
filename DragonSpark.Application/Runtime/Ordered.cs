using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Runtime
{
	sealed class Ordered<T> : IAlteration<Array<T>> where T : class, IOrderAware
	{
		public static Ordered<T> Default { get; } = new Ordered<T>();

		Ordered() {}

		public Array<T> Get(Array<T> parameter)
		{
			var order = 0u;
			foreach (var aware in parameter.Open().AsValueEnumerable())
			{
				aware.Order ??= order++;
			}

			return parameter;
		}
	}
}
using DragonSpark.Runtime;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Memory
{
	sealed class GenericCollectionLease<T> : ILease<ICollection<T>, T>
	{
		public static GenericCollectionLease<T> Default { get; } = new GenericCollectionLease<T>();

		GenericCollectionLease() {}

		public Lease<T> Get(ICollection<T> parameter)
		{
			using var builder = ArrayBuilder.New<T>(parameter.Count);
			builder.Add(parameter);
			var result = builder.AsLease();
			return result;
		}
	}
}
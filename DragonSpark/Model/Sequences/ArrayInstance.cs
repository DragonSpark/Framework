using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences
{
	public class ArrayInstance<T> : Instance<Array<T>>, IArray<T>
	{
		public ArrayInstance(IEnumerable<T> enumerable) : this(enumerable.Open()) {}

		public ArrayInstance(params T[] instance) : base(instance) {}
	}
}
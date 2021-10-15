using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences;

public class Instances<T> : Instance<Array<T>>, IArray<T>
{
	public Instances(IEnumerable<T> enumerable) : this(enumerable.Open()) {}

	public Instances(params T[] instance) : base(instance) {}
}
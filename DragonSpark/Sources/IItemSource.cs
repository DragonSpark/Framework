using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Sources
{
	public interface IItemSource<T> : ISource<ImmutableArray<T>>, IEnumerable<T> {}
}
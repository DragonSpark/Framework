using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources
{
	public class CompositeItemSource<T> : ItemSourceBase<T>
	{
		readonly ImmutableArray<IItemSource<T>> sources;

		public CompositeItemSource( params IItemSource<T>[] sources )
		{
			this.sources = sources.ToImmutableArray();
		}

		protected override IEnumerable<T> Yield() => sources.AsEnumerable().Concat();
	}
}
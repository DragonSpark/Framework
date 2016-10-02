using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Sources
{
	public abstract class ItemSourceBase<T> : SourceBase<ImmutableArray<T>>, IItemSource<T>
	{
		public sealed override ImmutableArray<T> Get() => Yield().ToImmutableArray();
		
		protected abstract IEnumerable<T> Yield();

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => Yield().GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Yield().GetEnumerator();
	}
}
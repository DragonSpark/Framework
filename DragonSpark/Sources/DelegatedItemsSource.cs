using System;
using System.Collections.Generic;

namespace DragonSpark.Sources
{
	public class DelegatedItemsSource<T> : ItemSourceBase<T>
	{
		readonly Func<IEnumerable<T>> source;
		public DelegatedItemsSource( Func<IEnumerable<T>> source )
		{
			this.source = source;
		}

		protected override IEnumerable<T> Yield() => source();
	}
}
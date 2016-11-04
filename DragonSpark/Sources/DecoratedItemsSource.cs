using System.Collections.Generic;

namespace DragonSpark.Sources
{
	public class DecoratedItemsSource<T> : DelegatedItemsSource<T>
	{
		public DecoratedItemsSource( ISource<IEnumerable<T>> scope ) : base( scope.Get ) {}
	}
}
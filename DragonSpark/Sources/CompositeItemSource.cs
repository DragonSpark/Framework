using DragonSpark.Extensions;
using System;
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

	public class DelegatedItemsSource<T> : ItemSourceBase<T>
	{
		readonly Func<IEnumerable<T>> source;
		public DelegatedItemsSource( Func<IEnumerable<T>> source )
		{
			this.source = source;
		}

		protected override IEnumerable<T> Yield() => source();
	}

	public class DecoratedItemsSource<T> : DelegatedItemsSource<T>
	{
		public DecoratedItemsSource( ISource<IEnumerable<T>> scope ) : base( scope.Get ) {}
	}
}
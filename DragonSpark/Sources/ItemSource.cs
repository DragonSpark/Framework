using DragonSpark.TypeSystem;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Sources
{
	public class ItemSource<T> : ItemSourceBase<T>
	{
		readonly IEnumerable<T> items;

		public ItemSource() : this( Items<T>.Default ) {}

		public ItemSource( params T[] items ) : this( items.AsEnumerable() ) {}

		public ItemSource( IEnumerable<T> items )
		{
			this.items = items;
		}

		protected override IEnumerable<T> Yield() => items;
	}
}
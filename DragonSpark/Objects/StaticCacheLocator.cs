using System.Collections.Generic;

namespace DragonSpark.Objects
{
	public class StaticCacheLocator<TKey,TObject> : LocatorBase<TKey,TObject> where TObject : class
	{
		readonly IEnumerable<TObject> cache;
		public StaticCacheLocator( IEnumerable<TObject> cache )
		{
			this.cache = cache;
		}

		protected override IEnumerable<TObject> List
		{
			get { return cache; }
		}
	}
}
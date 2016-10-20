using DragonSpark.Extensions;
using System.Collections.Generic;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class DictionaryCache<TKey, TValue> : CacheBase<TKey, TValue>
	{
		public DictionaryCache( IDictionary<TKey, TValue> dictionary )
		{
			Source = dictionary;
		}

		protected IDictionary<TKey, TValue> Source { get; }

		public override TValue Get( TKey parameter ) => Source.TryGet( parameter );

		public override void Set( TKey instance, TValue value ) => Source[instance] = value;
		public override bool Contains( TKey instance ) => Source.ContainsKey( instance );

		public override bool Remove( TKey instance ) => Source.Remove( instance );
	}
}

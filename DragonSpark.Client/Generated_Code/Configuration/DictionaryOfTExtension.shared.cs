using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Configuration
{
	public class DictionaryOfTExtension : CollectionOfTExtensionBase<IDictionary>
	{
		protected override Type GetCollectionType( Type type )
		{
			return typeof(Dictionary<,>).MakeGenericType( KeyType ?? typeof(Object), type );
		}

		public Type KeyType { get; set; }

		protected override void CopyItems( object oldItems )
		{
			var oldItemsAsDictionary = oldItems.To<IDictionary>();
			var newItemsAsDictionary = Items;
			oldItemsAsDictionary.Cast<DictionaryEntry>().Select( entry => entry.Key ).NotNull().Apply( x =>
			{
			    newItemsAsDictionary[ x ] = oldItemsAsDictionary[ x ];
			} );
		}
	}
}
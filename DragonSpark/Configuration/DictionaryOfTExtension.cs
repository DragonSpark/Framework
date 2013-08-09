using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	[ContentProperty( "Pairs" )]
	public class DictionaryOfTExtension : DictionaryOfTExtensionBase
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			Items.As<IDictionary>( x => Pairs.Apply( y => x.Add( y.Key.ConvertTo( KeyType ), y.Value.ConvertTo( TypeArgument ) ) ) );
			return base.ProvideValue( serviceProvider );
		}

		public Collection<KeyValuePair> Pairs
		{
			get { return pairs; }
		}	readonly Collection<KeyValuePair> pairs = new Collection<KeyValuePair>();
	}

	[ContentProperty( "Items" )]
	public class DictionaryOfTExtensionBase : CollectionOfTExtensionBase<IDictionary>
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

	[ContentProperty( "Value" )]
	public class KeyValuePair
	{
		public object Key { get; set; }
		
		public object Value { get; set; }
	}
}
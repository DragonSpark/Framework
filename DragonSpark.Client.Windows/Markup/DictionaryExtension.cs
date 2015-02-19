using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Markup
{
	[ContentProperty( "Pairs" )]
	public class DictionaryExtension : DictionaryExtensionBase
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
}
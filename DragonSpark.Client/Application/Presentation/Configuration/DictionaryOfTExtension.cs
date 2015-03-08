using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Presentation.Configuration
{
	[ContentProperty( "Pairs" )]
	public class DictionaryOfTExtension : DragonSpark.Configuration.DictionaryOfTExtension
	{
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			Items.As<IDictionary>( x => Pairs.Apply( y => x.Add( ConversionHelper.ConvertTo( y.Key, KeyType ), ConversionHelper.ConvertTo( y.Value, TypeArgument ) ) ) );
			return base.ProvideValue( serviceProvider );
		}

		public Collection<KeyValuePair> Pairs
		{
			get { return pairs; }
		}	readonly Collection<KeyValuePair> pairs = new Collection<KeyValuePair>();
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Markup
{
	[ContentProperty( "Items" )]
	public class ListExtension : CollectionExtensionBase<IList>
	{
		protected override Type GetCollectionType( Type type )
		{
			return typeof(List<>).MakeGenericType( type );
		}
	}
}
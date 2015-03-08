using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	[ContentProperty( "Items" )]
	public class ListOfTExtension : CollectionOfTExtensionBase<IList>
	{
		protected override Type GetCollectionType( Type type )
		{
			return typeof(List<>).MakeGenericType( type );
		}
	}
}
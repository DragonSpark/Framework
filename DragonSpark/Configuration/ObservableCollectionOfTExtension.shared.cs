using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	[ContentProperty( "Items" )]
	public class ObservableCollectionOfTExtension : CollectionOfTExtensionBase<IList>
	{
		protected override Type GetCollectionType( Type type )
		{
			return typeof(ObservableCollection<>).MakeGenericType( type );
		}
	}
}
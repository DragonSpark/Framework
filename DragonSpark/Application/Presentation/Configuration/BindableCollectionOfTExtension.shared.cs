using System.Collections;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Configuration;

namespace DragonSpark.Application.Presentation.Configuration
{
	[ContentProperty( "Items" )]
	public class BindableCollectionOfTExtension : CollectionOfTExtensionBase<IList>
	{
		protected override System.Type GetCollectionType( System.Type type )
		{
			return typeof(ViewCollection<>).MakeGenericType( type );
		}
	}
}
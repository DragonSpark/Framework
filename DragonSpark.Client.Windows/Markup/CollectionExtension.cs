using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DragonSpark.Markup
{
	[ContentProperty("Items")]
	public class CollectionExtension : CollectionExtensionBase<IList>
	{
		protected override Type GetCollectionType(Type type)
		{
			return typeof(Collection<>).MakeGenericType(type);
		}
	}
}